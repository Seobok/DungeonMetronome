using System.Collections.Generic;
using System.IO;
using Map;
using Unit.Enemy;
using UnityEngine;

namespace Utility
{
    public static class CsvReader
    {
        private static bool _isReadingEnemyData = false;
        
        public static Dictionary<string, EnemyData> EnemyData 
        {
            get
            {
              if(!_isReadingEnemyData)
                  LoadMonstersFromCsv();

              return _cachedEnemyData;
            }
        }
        private static Dictionary<string, EnemyData> _cachedEnemyData;
        
        private static void LoadMonstersFromCsv()
        {
            if (_isReadingEnemyData)
            {
                return;
            }

            _isReadingEnemyData = true;
            
            
            TextAsset csvFile = Resources.Load<TextAsset>("EnemyData");
            var monsterList = new Dictionary<string, EnemyData>();

            using (StringReader reader = new StringReader(csvFile.text)) {
                string line = reader.ReadLine(); // 헤더 스킵

                while ((line = reader.ReadLine()) != null) {
                    var split = line.Split(',');

                    var data = new EnemyData {
                        Id = ushort.Parse(split[0]),
                        Name = split[1],
                        Hp = int.Parse(split[2]),
                        DetectRange = int.Parse(split[3]),
                        MoveSpeed = int.Parse(split[4]),
                    };

                    monsterList.Add(data.Name, data);
                }
            }

            _cachedEnemyData = monsterList;
        }
    }
}