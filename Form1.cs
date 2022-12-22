using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinForm検証
{
    public class 列車利用実績
    {
        public int 年度 { get; set; }
        public string Name { get; set; }
        public int 利用者数 { get; set; }
        public int 年間_利用者数 { get; set; }

        public 列車利用実績()
        {
        }
        public 列車利用実績(int 年度, string name, int 利用者数, int 年間_利用者数=0)
        {
            this.年度 = 年度;
            Name = name;
            this.利用者数 = 利用者数;
            this.年間_利用者数 = 年間_利用者数;
        }

        public override string ToString()
        {
            var str = string.Format("年度=[{0}]  Name=[{1}]  利用者数=[{2}]  年間_利用者数=[{3}]", 年度, Name, 利用者数, 年間_利用者数);
            return str;
        }

    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var 実績s1 = new List<列車利用実績> {
                new 列車利用実績(2022, "aaa", 1),
                new 列車利用実績(2022, "bbb", 2),
                new 列車利用実績(2022, "ccc", 3),
            };

            var 実績s2 = new List<列車利用実績> {
                new 列車利用実績(2023, "ddd", 4),
                new 列車利用実績(2023, "eee", 5),
                new 列車利用実績(2024, "fff", 6),
            };

            var すべての実績 = 実績s1.Concat(実績s2).ToList();


            // 確認用 全件コンソール出力
            Console.WriteLine("■ 集計前の全件出力 ------------------ ");
            全件コンソール出力(すべての実績);

            // --------------------------------------------
            // 年度ごとに処理をするので、まずは年度を取得
            // --------------------------------------------
            List<int> 年度s = すべての実績
                .GroupBy(it => new { 年度 = it.年度 })
                .Select(g => g.Key.年度)
                .ToList();


            // --------------------------------------------
            // 年度ごとに集計＋更新処理
            // --------------------------------------------
            foreach (int year in 年度s)
            {
                // ---------- 対象年度の集計と更新 -------------
                int 年度合計 = すべての実績
                    .Where(it => it.年度 == year)    
                    .Sum(it => it .利用者数)
                    ;

                var 対象年度の集計された実績 = すべての実績
                    .Where(it => it.年度 == year)
                    .Select(it => new 列車利用実績(it.年度, it.Name, it.利用者数, 年度合計))
                    .ToList()
                    ;

                // ---------- 対象年度以外 -------------
                var 対象年度以外の実績 = すべての実績
                    .Where(it => it.年度 != year)
                    .ToList()
                    ;

                // ---------- マージ -------------
                すべての実績 = 対象年度以外の実績.Concat(対象年度の集計された実績)
                                .ToList();
            }

            // 確認用 全件コンソール出力
            Console.WriteLine("■ 集計後の全件出力 ------------------ ");
            全件コンソール出力(すべての実績);

        }

        private void 全件コンソール出力(List<列車利用実績> 実績s)
        {
            foreach (var 実績 in 実績s)
            {
                Console.WriteLine(実績.ToString());
            }
        }


    }
}
