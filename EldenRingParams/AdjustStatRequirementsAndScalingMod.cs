using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingParams
{
    internal class AdjustStatRequirementsAndScalingMod
    {
        private ModContext ModContext;

        // Param names
        private const string EquipParamWeapon = "EquipParamWeapon";
        private const string ReinforceParamWeapon = "ReinforceParamWeapon";
        private const string CalcCorrectGraph = "CalcCorrectGraph";
        private const string AttackElementCorrectParam = "AttackElementCorrectParam";

        public AdjustStatRequirementsAndScalingMod(ModContext modContext)
        {
            ModContext = modContext;
        }

        public void Run()
        {
            Console.WriteLine("Modifying weapons...");
            var weapons = ModContext.ParamsByName[EquipParamWeapon];
            var requirementCellNames = new string[] { "Requirement: STR", "Requirement: DEX", "Requirement: INT", "Requirement: FTH", "Requirement: ARC" };
            var dictionary = new Dictionary<string, bool>();

            foreach (var weapon in weapons.Rows)
            {
                // Set all weapon stat requirements to 0
                foreach (var cellName in requirementCellNames)
                {
                    var cell = ModContext.GetCellByDisplayName(weapon, cellName);
                    cell.Value = (byte)0;
                }

                try
                {
                    var maxUpgradeLevel = GetMaxUpgradeLevel(weapon);
                    var reinforceTypeId = GetReinforceTypeIdForUpgradeLevel(weapon, maxUpgradeLevel);
                    var maxReinforcementValues = ModContext.GetRowById(ReinforceParamWeapon, reinforceTypeId);

                    var statNames = new string[] { "STR", "DEX", "INT", "FTH", "ARC" };
                    for (int i = 0; i <= maxUpgradeLevel; i++)
                    {
                        reinforceTypeId = GetReinforceTypeIdForUpgradeLevel(weapon, i);
                        var currentReinforcementValues = ModContext.GetRowById(ReinforceParamWeapon, reinforceTypeId);
                        foreach (var statName in statNames)
                        {
                            var correctionPercentName = $"Correction %: {statName}";
                            var correctionPercent = ModContext.GetCellByDisplayName(currentReinforcementValues, correctionPercentName);
                            var key = reinforceTypeId + "|" + statName;
                            if (dictionary.ContainsKey(key))
                            {
                                continue;
                            }
                            dictionary.Add(key, true);
                            if ((float)correctionPercent.Value > 0)
                            {
                                correctionPercent.Value = (float)correctionPercent.Value * 4;
                                Console.WriteLine($"{ModContext.GetRowName("EquipParamWeapon", weapon.ID)} {statName} = {correctionPercent.Value}");
                            }
                        }
                    }
                }
                catch (Exception) {
                }

                // Just set weapons such that scaling is equal to the scaling when fully upgraded

                // Adjust scaling and damage
                //GuessWeaponAR(weapon, 0f, 10);
                //GuessWeaponAR(weapon, 0.5f, 10);
                //GuessWeaponAR(weapon, 1f, 10);

                //GuessWeaponAR(weapon, 0f, 40);
                //GuessWeaponAR(weapon, 0.5f, 40);
                //GuessWeaponAR(weapon, 1f, 40);
            }

            // Todo scale AR such that ideally when attr is 10, normal +0 dmg. when attr is say 40, +25/+10 dmg.
            // Correction is scaling
        }

        private void GuessWeaponAR(PARAM.Row weapon, float upgradePercent, int statLevel)
        {
            // Adjust scaling and damage
            var maxUpgradeLevel = GetMaxUpgradeLevel(weapon);
            if (maxUpgradeLevel <= 0)
            {
                return;
            }

            int upgradeLevel = (int)Math.Floor(upgradePercent * maxUpgradeLevel);
            //Console.WriteLine($"{ModContext.GetRowName(EquipParamWeapon, weapon.ID)} ({weapon.ID}) max upgrade level is +{maxUpgradeLevel}");

            var reinforceTypeId = GetReinforceTypeIdForUpgradeLevel(weapon, upgradeLevel);
            var currentReinforcementValues = ModContext.GetRowById(ReinforceParamWeapon, reinforceTypeId);


            //reinforceTypeId = GetReinforceTypeIdForUpgradeLevel(weapon, 0);
            //var minReinforcementValues = ModContext.GetRowById(ReinforceParamWeapon, reinforceTypeId);
            //reinforceTypeId = GetReinforceTypeIdForUpgradeLevel(weapon, maxUpgradeLevel);
            //var maxReinforcementValues = ModContext.GetRowById(ReinforceParamWeapon, reinforceTypeId);
            //Console.WriteLine($"{ModContext.GetCellByDisplayName(minReinforcementValues, "Damage %: Physical").Value} => {ModContext.GetCellByDisplayName(maxReinforcementValues, "Damage %: Physical").Value}");

            var damageTypeNames = new string[] { "Physical", "Magic", "Fire", "Lightning", "Holy" };

            Console.WriteLine($"{ModContext.GetRowName(EquipParamWeapon, weapon.ID)} ({weapon.ID}) +{upgradeLevel} all stats @ {statLevel}");
            float attackRating = 0;
            foreach (var damageType in damageTypeNames)
            {
                var damageCellName = $"Damage: {damageType}";
                var baseDamage = (ushort)ModContext.GetCellByDisplayName(weapon, damageCellName).Value;
                
                if (baseDamage > 0)
                {
                    var damagePercentCellName = $"Damage %: {damageType}";
                    var damagePercent = (float)ModContext.GetCellByDisplayName(currentReinforcementValues, damagePercentCellName).Value;
                    var adjustedBaseDamage = baseDamage * damagePercent;
                    var adjustedScalingDamage = GetAdjustedScalingDamageForWeapon(weapon, damageType, upgradeLevel, statLevel);
                    Console.WriteLine($"\t{damageType} = {adjustedBaseDamage} + {adjustedScalingDamage}");
                    attackRating += adjustedBaseDamage + adjustedScalingDamage;
                }
            }

            // CalcCorrectGraph
            // Attack Element Correct ID -> AttackElementCorrectParam

            Console.WriteLine($"\tAR = ~{attackRating}");
        }

        private float GetAdjustedScalingDamageForWeapon(PARAM.Row weapon, string damageType, int upgradeLevel, int statLevel)
        {
            var attackElementCorrectId = (int)ModContext.GetCellByDisplayName(weapon, "Attack Element Correct ID").Value;
            var attackElementCorrection = ModContext.GetRowById(AttackElementCorrectParam, attackElementCorrectId);

            var reinforceTypeId = GetReinforceTypeIdForUpgradeLevel(weapon, upgradeLevel);
            var currentReinforcementValues = ModContext.GetRowById(ReinforceParamWeapon, reinforceTypeId);

            var calcCorrectGraphId = (byte)ModContext.GetCellByDisplayName(weapon, $"Correction Type: {damageType}").Value;
            var calcCorrectGraph = ModContext.GetRowById(CalcCorrectGraph, calcCorrectGraphId);

            var statNames = new string[] { "STR", "DEX", "INT", "FTH", "ARC" };
            float total = 0;
            foreach (var statName in statNames)
            {
                var correctionEnabledCellName = $"{damageType} Correction: {statName}";
                var correctionEnabled = (byte)ModContext.GetCellByDisplayName(attackElementCorrection, correctionEnabledCellName).Value > 0;
                if (correctionEnabled)
                {
                    var correctionAmountName = $"Correction: {statName}";
                    var correctionAmount = (float)ModContext.GetCellByDisplayName(weapon, correctionAmountName).Value;

                    var correctionRatioCellName = $"{damageType} Correction Ratio: {statName}";
                    var correctionRatio = (short)ModContext.GetCellByDisplayName(attackElementCorrection, correctionRatioCellName).Value / 100f;

                    var correctionPercentCellName = $"Correction %: {statName}";
                    var correctionPercent = (float)ModContext.GetCellByDisplayName(currentReinforcementValues, correctionPercentCellName).Value;

                    Console.WriteLine($"\t\tBase = {correctionAmount}, Ratio = {correctionRatio}, Percent = {correctionPercent}");
                    var adjustedScalingDamage = AdjustByCalcCorrectGraph(correctionAmount * correctionRatio * correctionPercent, calcCorrectGraph) * statLevel;
                    total += adjustedScalingDamage;
                }
            }

            return total;
        }

        private float AdjustByCalcCorrectGraph(float value, PARAM.Row calcCorrectGraph)
        {
            for (int i = 0; i < 4; i++)
            {
                var thresholdPointMinName = $"Threshold Point [{i}]";
                var thresholdPointMin = (float)ModContext.GetCellByDisplayName(calcCorrectGraph, thresholdPointMinName).Value;

                var thresholdPointMaxName = $"Threshold Point [{i + 1}]";
                var thresholdPointMax = (float)ModContext.GetCellByDisplayName(calcCorrectGraph, thresholdPointMaxName).Value;

                if (value >= thresholdPointMin && value <= thresholdPointMax)
                {
                    var thresholdCoefficientMinName = $"Threshold Coefficient [{i}]";
                    var thresholdCoefficientMin = (float)ModContext.GetCellByDisplayName(calcCorrectGraph, thresholdCoefficientMinName).Value;

                    var thresholdCoefficientMaxName = $"Threshold Coefficient [{i + 1}]";
                    var thresholdCoefficientMax = (float)ModContext.GetCellByDisplayName(calcCorrectGraph, thresholdCoefficientMaxName).Value;

                    var alpha = InverseLerpUnclamped(thresholdPointMin, thresholdPointMax, value);
                    var coefficient = LerpClamped(thresholdCoefficientMin, thresholdCoefficientMax, alpha) / 100f;

                    return value * coefficient;
                }
            }

            return 0;
        }

        private static float InverseLerpUnclamped(float a, float b, float t)
        {
            return (t - a) / (b - a);
        }

        private static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private static float LerpClamped(float a, float b, float t)
        {
            return LerpUnclamped(a, b, Math.Min(Math.Max(t, 0), 1));
        }

        private int GetReinforceTypeIdForUpgradeLevel(PARAM.Row weapon, int upgradeLevel)
        {
            var originWeaponId = ModContext.GetCellByDisplayName(weapon, $"Origin Weapon +{upgradeLevel}");
            var originWeapon = ModContext.GetRowById(EquipParamWeapon, (int)originWeaponId.Value);
            var reinforceTypeId = ModContext.GetCellByDisplayName(originWeapon, "Reinforce Type ID");
            return (short)reinforceTypeId.Value + upgradeLevel;
        }

        private int GetMaxUpgradeLevel(PARAM.Row weapon)
        {
            int maxUpgradeLevel = 0;
            for (int i = 1; i <= 25; i++)
            {
                var cell = ModContext.GetCellByDisplayName(weapon, $"Origin Weapon +{i}");
                if (cell != null)
                {
                    var row = ModContext.GetRowById(EquipParamWeapon, (int)cell.Value);
                    if (row != null)
                    {
                        maxUpgradeLevel = i;
                    }
                }
            }

            return maxUpgradeLevel;
        }
    }
}
