using System;
using System.Linq;


namespace wilcoxon
{
    class Program
    {
        static double SpearmanTest(double[] y1, double[] y2)
        {
            double coefSpearman = 0;
            double[] delta = new double[y1.Length];
            double sumDelta = 0;
            var y1ToRank = y1
               .OrderBy(x => x)// отсортировали по возрастанию
               .Select((x, idx) => (x, rank: idx + 1))// раздали ранги
               .GroupBy(p => p.x)// сгруппировали одинаковые значения
               .ToDictionary(g => g.Key, g => g.Average(p => p.rank));// посчитали средний ранг по группе
            var y1Ranks = y1
                .Select(x => y1ToRank[x])
                .ToArray();
            var y2ToRank = y2
               .OrderBy(x => x)// отсортировали по возрастанию
               .Select((x, idx) => (x, rank: idx + 1))// раздали ранги
               .GroupBy(p => p.x)// сгруппировали одинаковые значения
               .ToDictionary(g => g.Key, g => g.Average(p => p.rank));// посчитали средний ранг по группе
            var y2Ranks = y2
                .Select(x => y2ToRank[x])
                .ToArray();
            for (int i = 0; i < y1.Length; i++)
            {
                delta[i] = y1Ranks[i] - y2Ranks[i];
                sumDelta += delta[i];
            }
            coefSpearman = 1 - ((6 * sumDelta) / (delta.Length * (Math.Pow(delta.Length, 2) - 1)));
            return coefSpearman;
        }
        static double AdvancedWilcoxonTest(double[] y1, double[]y2,double sigma0, double alpha)
        {
            
            var delta = new double[y1.Length];
            var absDelta = new double[y1.Length];
            double positiveSum = 0;
            double negativeSum = 0;
            for (int i=0;i<y1.Length; i++)
            {
                //Standart signed rank test Wilcoxon(t-wilcoxon)
                //delta[i] = y1[i] - y2[i];
                //absDelta[i] = Math.Abs(delta[i]);
                //Advanced signed rank test Wilcoxon
                delta[i] = Math.Abs(y1[i] - y2[i])-sigma0;
                absDelta[i] = Math.Abs(delta[i]);
            }

            var valueToRank = absDelta
                .OrderBy(x => x)// отсортировали по возрастанию
                .Select((x, idx) => (x, rank: idx + 1))// раздали ранги
                .GroupBy(p => p.x)// сгруппировали одинаковые значения
                .ToDictionary(g => g.Key, g => g.Average(p => p.rank));// посчитали средний ранг по группе
            var ranks = absDelta
                .Select(x => valueToRank[x])
                .ToArray();

            for (int i= 0; i <y1.Length; i++)
            {
                
                if (delta[i]>0)
                {
                    positiveSum += ranks[i];
                }
                else
                {
                    negativeSum += ranks[i];
                }
                Console.WriteLine($"absDelta[{i}]={absDelta[i]}");
                Console.WriteLine($"ranks[{i}]={ranks[i]}"); 
            }
            Console.WriteLine($"positiveSum={positiveSum}");
            return positiveSum;
            
        }
        static void Main(string[] args)
        {
            double[] x = new double[]{ 12035, 12213, 11893, 11920, 11448, 10768, 10233,12549,12728,12578,12114,12055,11841,11492,10620,12867,12530,12776,12435,12485,12321,10637,10056,12188};
            double[] y = new double[]{ 13456,13464,13069,12774,12561,11778,10353,12698,13861,13824,12737,12952,13503,13372,11886,13479,13293,13717,12772,12495,12551,12085,10311,13001};
            AdvancedWilcoxonTest(x, y, 1103, 0.05);
        }

    }
}
