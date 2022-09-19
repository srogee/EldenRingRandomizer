using System;
using Keystone;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyHook;

namespace EldenRingItemRandomizer.GameState
{
    internal class EldenRingHook : PHook
    {
        private PHPointer GameDataMan { get; set; }
        private PHPointer GameMan { get; set; }
        private PHPointer PlayerGameData { get; set; }
        private PHPointer ClassWhereTheNameIsStored { get; set; }
        private PHPointer PlayerInventory { get; set; }
        private PHPointer HeldNormalItemsPtr { get; set; }
        private PHPointer HeldSpecialItemsPtr { get; set; }
        private PHPointer SoloParamRepository { get; set; }
        private PHPointer CapParamCall { get; set; }
        public PHPointer ItemGive { get; set; }
        public PHPointer MapItemMan { get; set; }
        public PHPointer EventFlagMan { get; set; }
        public PHPointer SetEventFlagFunction { get; set; }
        public PHPointer IsEventFlagFunction { get; set; }
        public PHPointer WorldChrMan { get; set; }
        public PHPointer PlayerIns { get; set; }
        public PHPointer DisableOpenMap { get; set; }
        public PHPointer CombatCloseMap { get; set; }
        public PHPointer WorldAreaWeather { get; set; }
        public PHPointer CSFD4VirtualMemoryFlag { get; set; }
        public PHPointer CSLuaEventManager { get; set; }
        public PHPointer LuaWarp_01AoB { get; set; }
        public PHPointer Crash { get; set; }

        public bool Loaded => PlayerIns != null ? PlayerIns.Resolve() != IntPtr.Zero : false;

        public EldenRingHook(int refreshInterval, int minLifetime, Func<Process, bool> processSelector) : base(refreshInterval, minLifetime, processSelector)
        {
            OnHooked += EldenRingHook_OnHooked;
            OnUnhooked += EldenRingHook_OnUnhooked;

            GameDataMan = RegisterRelativeAOB(Offsets.GameDataManAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize, 0x0);
            ClassWhereTheNameIsStored = CreateChildPointer(GameDataMan, (int)Offsets.GameDataMan.ClassWhereTheNameIsStored);
            GameMan = RegisterRelativeAOB(Offsets.GameManAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize, 0x0);
            PlayerGameData = CreateChildPointer(GameDataMan, Offsets.PlayerGameData);
            PlayerInventory = CreateChildPointer(PlayerGameData, Offsets.EquipInventoryDataOffset, Offsets.PlayerInventoryOffset);
            HeldNormalItemsPtr = CreateChildPointer(PlayerGameData, (int)Offsets.PlayerGameDataStruct.HeldNormalItems);
            HeldSpecialItemsPtr = CreateChildPointer(PlayerGameData, (int)Offsets.PlayerGameDataStruct.HeldSpecialItems);

            SoloParamRepository = RegisterRelativeAOB(Offsets.SoloParamRepositoryAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize, 0x0);

            ItemGive = RegisterAbsoluteAOB(Offsets.ItemGiveAoB);
            MapItemMan = RegisterRelativeAOB(Offsets.MapItemManAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize);
            EventFlagMan = RegisterRelativeAOB(Offsets.EventFlagManAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize, 0x0);
            SetEventFlagFunction = RegisterAbsoluteAOB(Offsets.SetEventCallAoB);
            IsEventFlagFunction = RegisterAbsoluteAOB(Offsets.IsEventCallAoB);

            CapParamCall = RegisterAbsoluteAOB(Offsets.CapParamCallAoB);

            WorldChrMan = RegisterRelativeAOB(Offsets.WorldChrManAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize, 0x0);
            PlayerIns = CreateChildPointer(WorldChrMan, Offsets.PlayerInsOffset);

            DisableOpenMap = RegisterAbsoluteAOB(Offsets.DisableOpenMapAoB);
            CombatCloseMap = RegisterAbsoluteAOB(Offsets.CombatCloseMapAoB);
            WorldAreaWeather = RegisterRelativeAOB(Offsets.WorldAreaWeatherAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize, 0x0);

            CSFD4VirtualMemoryFlag = RegisterRelativeAOB(Offsets.CSFD4VirtualMemoryFlagAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize, 0x0);
            CSLuaEventManager = RegisterRelativeAOB(Offsets.CSLuaEventManagerAoB, Offsets.RelativePtrAddressOffset, Offsets.LargeRelativePtrInstructionSize);
            LuaWarp_01AoB = RegisterAbsoluteAOB(Offsets.LuaWarp_01AoB);
        }

        private void EldenRingHook_OnUnhooked(object sender, PHEventArgs e)
        {
        }

        private void EldenRingHook_OnHooked(object sender, PHEventArgs e)
        {
        }

        public void GiveItem(ItemSpawnInfo item)
        {
            byte[] itemInfobytes = new byte[(int)Offsets.ItemGiveStruct.ItemStructHeaderSize + (int)Offsets.ItemGiveStruct.ItemStructEntrySize];
            IntPtr itemInfo = GetPrefferedIntPtr(itemInfobytes.Length);

            byte[] bytes = BitConverter.GetBytes(0x1);
            Array.Copy(bytes, 0x0, itemInfobytes, (int)Offsets.ItemGiveStruct.Count, bytes.Length);

            bytes = BitConverter.GetBytes(item.ID + item.Infusion + item.Upgrade + (int)item.Category);
            Array.Copy(bytes, 0x0, itemInfobytes, (int)Offsets.ItemGiveStruct.ID, bytes.Length);

            bytes = BitConverter.GetBytes(item.Quantity);
            Array.Copy(bytes, 0x0, itemInfobytes, (int)Offsets.ItemGiveStruct.Quantity, bytes.Length);

            bytes = BitConverter.GetBytes(item.Gem);
            Array.Copy(bytes, 0x0, itemInfobytes, (int)Offsets.ItemGiveStruct.Gem, bytes.Length);

            Kernel32.WriteBytes(Handle, itemInfo, itemInfobytes);

            string asmString = Util.GetEmbededResource("Assembly.ItemGib.asm");
            string asm = string.Format(asmString, itemInfo.ToString("X2"), MapItemMan.Resolve(), ItemGive.Resolve() + Offsets.ItemGiveOffset);
            AsmExecute(asm);
            Free(itemInfo);

            if (item.EventID != -1)
                SetEventFlag(item.EventID, true);
        }

        public bool GetEventFlag(int flag)
        {
            IntPtr returnPtr = GetPrefferedIntPtr(sizeof(bool));
            IntPtr idPointer = GetPrefferedIntPtr(sizeof(int));
            Kernel32.WriteInt32(Handle, idPointer, flag);

            string asmString = Util.GetEmbededResource("Assembly.IsEventFlag.asm");
            string asm = string.Format(asmString, EventFlagMan.Resolve(), idPointer.ToString("X2"), IsEventFlagFunction.Resolve(), returnPtr.ToString("X2"));

            AsmExecute(asm);
            bool state = Kernel32.ReadBoolean(Handle, returnPtr);
            Free(returnPtr);
            Free(idPointer);

            return state;
        }

        public void SetEventFlag(int flag, bool state)
        {
            IntPtr idPointer = GetPrefferedIntPtr(sizeof(int));
            Kernel32.WriteInt32(Handle, idPointer, flag);

            string asmString = Util.GetEmbededResource("Assembly.SetEventFlag.asm");
            string asm = string.Format(asmString, EventFlagMan.Resolve(), (state ? 1 : 0), idPointer.ToString("X2"), SetEventFlagFunction.Resolve());
            AsmExecute(asm);
            Free(idPointer);
        }

        public bool GetBitFlag(int bitStart, bool value, params int[] offsets)
        {
            var pointer = CreateChildPointer(CSFD4VirtualMemoryFlag, offsets);
            var byteValue = pointer.ReadByte(0);
            return (byteValue & (1 << bitStart)) != 0;
        }

        public void SetBitFlag(int bitStart, bool value, params int[] offsets)
        {
            var pointer = CreateChildPointer(CSFD4VirtualMemoryFlag, offsets);
            var byteValue = pointer.ReadByte(0);
        }

        public void Warp(int bonfireID)
        {
            IntPtr warpLocation = GetPrefferedIntPtr(sizeof(int));
            Kernel32.WriteInt32(Handle, warpLocation, bonfireID);

            string asmString = Util.GetEmbededResource("Assembly.Warp.asm");
            string asm = string.Format(asmString, CSLuaEventManager.Resolve(), bonfireID, LuaWarp_01AoB.Resolve() + 2);
            AsmExecute(asm);
        }

        private Engine Engine = new(Architecture.X86, Mode.X64);
        //TKCode
        private void AsmExecute(string asm)
        {
            //Assemble once to get the size
            EncodedData? bytes = Engine.Assemble(asm, (ulong)Process.MainModule.BaseAddress);
            //DebugPrintArray(bytes.Buffer);
            KeystoneError error = Engine.GetLastKeystoneError();
            if (error != KeystoneError.KS_ERR_OK)
                throw new("Something went wrong during assembly. Code could not be assembled.");

            IntPtr insertPtr = GetPrefferedIntPtr(bytes.Buffer.Length, flProtect: Kernel32.PAGE_EXECUTE_READWRITE);

            //Reassemble with the location of the isertPtr to support relative instructions
            bytes = Engine.Assemble(asm, (ulong)insertPtr);
            error = Engine.GetLastKeystoneError();

            Kernel32.WriteBytes(Handle, insertPtr, bytes.Buffer);

            Execute(insertPtr);
            Free(insertPtr);
        }
    }
}
