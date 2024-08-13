using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.ObjectModel;

namespace Agit.FortressCraft
{

    public static class GoogleSheetManager
    {
        public static List<CommanderData> commanderDatas = new List<CommanderData>();
        public static List<UnitData> unitDatas = new List<UnitData>();

        // 링크 뒤 edit ~ 부분을 빼고 export?format=tsv 추가하기

        private static readonly List<string> _urls = new List<string>
        {
            "https://docs.google.com/spreadsheets/d/1Ngc05MXFcm1ZRdU2JH1iBRJm9uIPmVFJ32h_R3LrDoM/edit?gid=0#gid=0"
        };

        public static readonly ReadOnlyCollection<string> Urls = new ReadOnlyCollection<string>(_urls);

        public static IEnumerator Loader()
        {
            List<string> data = new List<string>();
            foreach (var url in Urls)
            {
                UnityWebRequest www = UnityWebRequest.Get(url);
                yield return www.SendWebRequest();

                data.Add(www.downloadHandler.text);

            }

            if (data[0] != null)
                ParseCommanderData(data[0]);
            if (data[1] != null)
                ParseUnitData(data[1]);
            foreach (string d in data)
            {
                Debug.Log(d);
            }
        }


        private static void ParseCommanderData(string data)
        {
            string[] lines = data.Split('\n');
            for (int i = 1; i < lines.Length; i++) // 첫 번째 행은 제목 행으로 생략
            {
                string[] fields = lines[i].Split('\t');
                if (fields.Length < 5) // 필요한 필드 수 미달 시 로그 출력 및 처리 중단
                {
                    Debug.LogError($"Line {i} has insufficient fields: {fields.Length} fields found.");
                    continue;
                }
                try
                {
                    CommanderData commanderData = new CommanderData()
                    {
                        CommanderType = fields[0],
                        Level = ParseInt(fields[1]),
                        NeedExp = ParseInt(fields[2]),
                        HP = ParseFloat(fields[3]),
                        MP = ParseFloat(fields[4]),
                        Attack = ParseFloat(fields[5]),
                        AttackSpeed = ParseFloat(fields[6]),
                        AttackDelay = ParseFloat(fields[7]),
                        Defense = ParseFloat(fields[8]),
                        MoveSpeed = ParseFloat(fields[9]),
                        HealPerSecond = ParseFloat(fields[10]),
                    };
                    commanderDatas.Add(commanderData);
                }
                catch (FormatException e)
                {
                    Debug.LogError($"FormatException on line {i}: {e.Message}");
                }
                catch (OverflowException e)
                {
                    Debug.LogError($"OverflowException on line {i}: {e.Message}");
                }
            }
        }
        private static void ParseUnitData(string data)
        {
            string[] lines = data.Split('\n');
            for (int i = 1; i < lines.Length; i++) // 첫 번째 행은 제목 행으로 생략
            {
                string[] fields = lines[i].Split('\t');
                if (fields.Length < 11) // 필요한 필드 수 미달 시 로그 출력 및 처리 중단
                {
                    Debug.LogError($"Line {i} has insufficient fields: {fields.Length} fields found.");
                    continue;
                }

                try
                {
                    UnitData unitData = new UnitData()
                    {
                        Level = ParseInt(fields[0]),
                        UpgradeCost = ParseInt(fields[1]),
                        HP = ParseFloat(fields[2]),
                        MP = ParseFloat(fields[3]),
                        Attack = ParseFloat(fields[4]),
                        AttackDelay = ParseFloat(fields[5]),
                        Defense = ParseFloat(fields[6]),
                        MoveSpeed = ParseFloat(fields[7]),
                        SpawnDelay = ParseFloat(fields[8]),
                        CostReward = ParseInt(fields[9]),
                        ExpReward = ParseInt(fields[10]),
                    };
                    unitDatas.Add(unitData);
                }
                catch (FormatException e)
                {
                    Debug.LogError($"FormatException on line {i}: {e.Message}");
                }
                catch (OverflowException e)
                {
                    Debug.LogError($"OverflowException on line {i}: {e.Message}");
                }
            }
        }
        private static int ParseInt(string input)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            throw new FormatException($"Unable to parse '{input}' as integer.");
        }

        private static float ParseFloat(string input)
        {
            if (float.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            throw new FormatException($"Unable to parse '{input}' as float.");
        }
    }

    [System.Serializable]
    public class CommanderData
    {
        public string CommanderType;
        public int Level;
        public int NeedExp;
        public float HP;
        public float MP;
        public float Attack;
        public float AttackSpeed;
        public float AttackDelay;
        public float Defense;
        public float MoveSpeed;
        public float HealPerSecond;
    }

    [System.Serializable]
    public class UnitData
    {
        public int Level;
        public int UpgradeCost;
        public float HP;
        public float MP;
        public float Attack;
        public float AttackDelay;
        public float Defense;
        public float MoveSpeed;
        public float SpawnDelay;
        public int CostReward;
        public int ExpReward;
    }
}