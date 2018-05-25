using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KGOOS_Web.Common
{
    public class Freight
    {
        public static string ceil = "ceil"; //向上取整
        public static string floor = "floor";//向下取整
        public static string round = "round";//不变


        #region 根据重量，公式获取运费
        /// <summary>
        /// 根据重量，公式获取运费
        /// </summary>
        /// <param name="W">重量</param>
        /// <param name="formula">公式</param>
        /// <returns></returns>
        public static float Count_Freight(float W, string formula)
        {

            float freight = 0;
            string formula_new = "";

            formula_new = formula;

            //替换公式中的 W
            formula_new = formula_new.Replace("W", W.ToString());
            formula_new = formula_new.Replace("w", W.ToString());

            formula_new = returnStr(formula_new, ceil);
            formula_new = returnStr(formula_new, floor);
            formula_new = returnStr(formula_new, round);

            freight = float.Parse(asmdbResult(formula_new));

            return freight;
        }
        #endregion

        #region 过滤掉 ceil，floor, round
        /// <summary>
        /// 过滤掉 ceil，floor, round
        /// </summary>
        /// <param name="formula">表达式</param>
        /// <param name="fun">ceil/floor/round</param>
        /// <returns></returns>
        private static string returnStr(string formula, string fun)
        {
            string formula_new = "";
            if (formula.Contains(fun))
            {
                string[] splitOne = System.Text.RegularExpressions.Regex.Split(formula, fun);
                //第一段保留，后面每个都有计算一次

                formula_new += splitOne[0];
                for (int i = 1; i < splitOne.Length; i++)
                {
                    string strSplitOne = "";
                    strSplitOne = splitOne[i];
                    //去掉ceil的前括号
                    splitOne[i] = splitOne[i].Substring(1);
                    //判断是ceil后面括号的位置
                    int n = splitOne[i].IndexOf(')');
                    //分为两个部分，目的把ceil里面的数值算出来
                    string strFront = splitOne[i].Substring(0, n);
                    n++;//n++的目的是去掉后括号
                    string strLatter = splitOne[i].Substring(n);

                    //前部分算出值
                    float sum = 0;
                    int ceil_sum = 0;
                    sum = float.Parse(asmdResult(strFront));

                    //判断向上取值，向下取值 或 原值
                    if (fun.Equals(ceil))
                    {
                        if (sum % 1 == 0)
                        {
                            ceil_sum = (int)sum;
                        }
                        else
                        {
                            ceil_sum = (int)sum + 1;
                        }

                        //拼回原字符串数组
                        splitOne[i] = ceil_sum + strLatter;
                    }
                    else if (fun.Equals(floor))
                    {
                        ceil_sum = (int)sum;

                        //拼回原字符串数组
                        splitOne[i] = ceil_sum + strLatter;
                    }
                    else if (fun.Equals(round))
                    {
                        //拼回原字符串数组
                        splitOne[i] = sum + strLatter;
                    }

                    //拼回原公式内
                    formula_new += splitOne[i];
                }
            }
            else
            {
                formula_new = formula;
            }
            return formula_new;
        }
        #endregion

        #region 带括号的四则混合运算
        private static string asmdbResult(string inputstr)
        {
            int left_b_signTemp = inputstr.LastIndexOf("(");
            while (left_b_signTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, left_b_signTemp);
                string inputstr2 = inputstr.Substring(left_b_signTemp + 1);
                int right_b_signTemp = inputstr2.IndexOf(")");
                string inputstr2_1 = inputstr2.Substring(0, right_b_signTemp);
                string inputstr2_2 = inputstr2.Substring(right_b_signTemp + 1);
                inputstr = inputstr1 + asmdbResult(inputstr2_1) + inputstr2_2;
                left_b_signTemp = inputstr.IndexOf("(");
            }
            inputstr = asmdResult(inputstr);
            return inputstr;
        }
        #endregion

        #region 四则混合运算
        private static string asmdResult(string inputstr)
        {
            #region "+" "-" "×" "÷" "÷-" "×-" 符号判断初始化
            int addSignTemp = inputstr.IndexOf("+");
            int subSignTemp = inputstr.IndexOf("-");
            int mulSignTemp = inputstr.IndexOf("×");
            int divSignTemp = inputstr.IndexOf("÷");
            int divSubSignTemp = inputstr.IndexOf("÷-");
            int mulSubSignTemp = inputstr.IndexOf("×-");
            #endregion
            #region "÷-"运算
            while (divSubSignTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, divSubSignTemp);
                int addSignTemp1 = inputstr1.LastIndexOf("+");
                int subSignTemp1 = inputstr1.LastIndexOf("-");
                int mulSignTemp1 = inputstr1.LastIndexOf("×");
                int divSignTemp1 = inputstr1.LastIndexOf("÷");
                int mulSubSignTemp1 = inputstr1.IndexOf("×-");
                int maxSignTemp = maxNum_5(addSignTemp1, subSignTemp1, mulSignTemp1, divSignTemp1, mulSubSignTemp1);
                //MessageBox.Show(maxSignTemp.ToString());
                string inputstr1_1 = "";
                string inputstr1_2 = "";
                if (maxSignTemp == -1)
                {
                    inputstr1_1 = "";
                    inputstr1_2 = inputstr1;
                }
                else
                {
                    if (maxSignTemp == mulSubSignTemp1)
                    {
                        inputstr1_1 = inputstr1.Substring(0, subSignTemp);
                        inputstr1_2 = inputstr1.Substring(subSignTemp);
                    }
                    else
                    {
                        inputstr1_1 = inputstr1.Substring(0, maxSignTemp + 1);
                        inputstr1_2 = inputstr1.Substring(maxSignTemp + 1);
                    }
                }
                string inputstr2 = inputstr.Substring(divSubSignTemp + 2);
                //MessageBox.Show(inputstr2);
                int addSignTemp2 = inputstr2.IndexOf("+");
                //MessageBox.Show(addSignTemp2.ToString());//1
                int subSignTemp2 = inputstr2.IndexOf("-");
                //MessageBox.Show(subSignTemp2.ToString());//0
                int mulSignTemp2 = inputstr2.IndexOf("×");
                //MessageBox.Show(mulSignTemp2.ToString());//0
                int divSignTemp2 = inputstr2.IndexOf("÷");
                //MessageBox.Show(divSignTemp2.ToString());//1
                int mulSubSignTemp2 = inputstr2.IndexOf("×-");
                //MessageBox.Show(mulSubSignTemp2.ToString());//0
                int minSignTemp = minNum(addSignTemp2, subSignTemp2, mulSignTemp2, divSignTemp2, mulSubSignTemp2);
                //MessageBox.Show(minSignTemp.ToString());
                string inputstr2_1 = "";
                string inputstr2_2 = "";
                if (minSignTemp == -1)
                {
                    inputstr2_1 = inputstr2;
                    inputstr2_2 = "";
                }
                else
                {
                    if (minSignTemp == mulSubSignTemp2)
                    {
                        inputstr2_1 = inputstr2.Substring(0, minSignTemp);
                        inputstr2_2 = inputstr2.Substring(minSignTemp);
                    }
                    else
                    {
                        inputstr2_1 = inputstr2.Substring(0, minSignTemp);
                        inputstr2_2 = inputstr2.Substring(minSignTemp);
                    }
                }
                string inputstr1_2_2_1 = inputstr1_2 + ("÷-") + inputstr2_1;
                //MessageBox.Show(inputstr2_1);
                //MessageBox.Show(inputstr1_2_2_1);
                //MessageBox.Show(inputstr2_2);
                //MessageBox.Show("inputstr1_2_2_1====" + inputstr1_2_2_1);
                inputstr1_2_2_1 = divResult(inputstr1_2_2_1);
                //MessageBox.Show(inputstr1_2_2_1);
                inputstr = inputstr1_1 + inputstr1_2_2_1 + inputstr2_2;
                //MessageBox.Show(inputstr);
                divSubSignTemp = inputstr.IndexOf("÷-");
                //MessageBox.Show(divSubSignTemp.ToString());
            }
            #endregion
            //MessageBox.Show("除减后"+inputstr);
            #region 过滤 -- +- ÷+ ++ ×+
            inputstr = filtSign(inputstr);
            #endregion
            //MessageBox.Show("除减过滤后" + inputstr);
            #region "×-"运算
            addSignTemp = inputstr.IndexOf("+");
            subSignTemp = inputstr.IndexOf("-");
            mulSignTemp = inputstr.IndexOf("×");
            divSignTemp = inputstr.IndexOf("÷");
            divSubSignTemp = inputstr.IndexOf("÷-");
            mulSubSignTemp = inputstr.IndexOf("×-");
            while (mulSubSignTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, mulSubSignTemp);
                int addSignTemp1 = inputstr1.LastIndexOf("+");
                int subSignTemp1 = inputstr1.LastIndexOf("-");
                int mulSignTemp1 = inputstr1.LastIndexOf("×");
                int divSignTemp1 = inputstr1.LastIndexOf("÷");
                int divSubSignTemp1 = -1;
                int maxSignTemp = maxNum_5(addSignTemp1, subSignTemp1, mulSignTemp1, divSignTemp1, divSubSignTemp1);
                //MessageBox.Show(maxSignTemp.ToString());
                string inputstr1_1 = "";
                string inputstr1_2 = "";
                if (maxSignTemp == -1)
                {
                    inputstr1_1 = "";
                    inputstr1_2 = inputstr1;
                }
                else
                {
                    if (maxSignTemp == divSubSignTemp1)
                    {
                        inputstr1_1 = inputstr1.Substring(0, subSignTemp);
                        inputstr1_2 = inputstr1.Substring(subSignTemp);
                    }
                    else
                    {
                        inputstr1_1 = inputstr1.Substring(0, maxSignTemp + 1);
                        inputstr1_2 = inputstr1.Substring(maxSignTemp + 1);
                    }
                }
                string inputstr2 = inputstr.Substring(mulSubSignTemp + 2);
                int addSignTemp2 = inputstr2.IndexOf("+");
                int subSignTemp2 = inputstr2.IndexOf("-");
                int mulSignTemp2 = inputstr2.IndexOf("×");
                int divSignTemp2 = inputstr2.IndexOf("÷");
                int divSubSignTemp2 = -1;
                int minSignTemp = minNum(addSignTemp2, subSignTemp2, mulSignTemp2, divSignTemp2, divSubSignTemp2);
                //MessageBox.Show(minSignTemp.ToString());
                string inputstr2_1 = "";
                string inputstr2_2 = "";
                if (minSignTemp == -1)
                {
                    inputstr2_1 = inputstr2;
                    inputstr2_2 = "";
                }
                else
                {
                    if (minSignTemp == divSubSignTemp2)
                    {
                        inputstr2_1 = inputstr2.Substring(0, minSignTemp);
                        inputstr2_2 = inputstr2.Substring(minSignTemp);
                    }
                    else
                    {
                        inputstr2_1 = inputstr2.Substring(0, minSignTemp);
                        inputstr2_2 = inputstr2.Substring(minSignTemp);
                    }
                }
                string inputstr1_2_2_1 = inputstr1_2 + "×-" + inputstr2_1;
                //MessageBox.Show(inputstr1_1);
                //MessageBox.Show("inputstr1_2_2_1:" + inputstr1_2_2_1);
                //MessageBox.Show(inputstr2_2);
                inputstr1_2_2_1 = mulResult(inputstr1_2_2_1);
                inputstr = inputstr1_1 + inputstr1_2_2_1 + inputstr2_2;
                //MessageBox.Show(inputstr);
                mulSubSignTemp = inputstr.IndexOf("×-");
            }
            #endregion
            //MessageBox.Show("乘减后" + inputstr);
            #region 过滤 -- +- ÷+ ++ ×+
            inputstr = filtSign(inputstr);
            #endregion
            //MessageBox.Show("乘减过滤后" + inputstr);
            #region 除运算
            #region "+" "-" "×" "÷" 符号判断初始化
            addSignTemp = inputstr.IndexOf("+");
            subSignTemp = inputstr.IndexOf("-");
            mulSignTemp = inputstr.IndexOf("×");
            divSignTemp = inputstr.IndexOf("÷");
            #endregion
            while (divSignTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, divSignTemp);
                int addSignTemp1 = inputstr1.LastIndexOf("+");
                int subSignTemp1 = inputstr1.LastIndexOf("-");
                int mulSignTemp1 = inputstr1.LastIndexOf("×");
                int maxNum = maxNum_3(addSignTemp1, subSignTemp1, mulSignTemp1);
                string inputstr1_1 = "";
                string inputstr1_2 = "";
                if (maxNum == -1)
                {
                    inputstr1_1 = "";
                    inputstr1_2 = inputstr1;
                }
                else
                {
                    inputstr1_1 = inputstr1.Substring(0, maxNum + 1);
                    inputstr1_2 = inputstr1.Substring(maxNum + 1);
                }
                string inputstr2 = inputstr.Substring(divSignTemp + 1);
                int addSignTemp2 = inputstr2.IndexOf("+");
                int subSignTemp2 = inputstr2.IndexOf("-");
                int mulSignTemp2 = inputstr2.IndexOf("×");
                int minNum_3 = minNum(addSignTemp2, subSignTemp2, mulSignTemp2, -1, -1);
                string inputstr2_1 = "";
                string inputstr2_2 = "";
                if (minNum_3 == -1)
                {
                    inputstr2_1 = inputstr2;
                    inputstr2_2 = "";
                }
                else
                {
                    inputstr2_1 = inputstr2.Substring(0, minNum_3);
                    inputstr2_2 = inputstr2.Substring(minNum_3);
                }
                string inputstr1_2_2_1 = inputstr1_2 + "÷" + inputstr2_1;
                //MessageBox.Show("inputstr1_2_2_1:" + inputstr1_2_2_1);
                inputstr = inputstr1_1 + divResult(inputstr1_2_2_1) + inputstr2_2;
                //MessageBox.Show(inputstr1_1);
                //MessageBox.Show(inputstr2_2);
                divSignTemp = inputstr.IndexOf("÷");
                //MessageBox.Show("inputstr" + inputstr);
            }
            #endregion
            //MessageBox.Show("除后" + inputstr);
            #region 乘运算
            #region "+" "-" "×" 符号判断初始化
            addSignTemp = inputstr.IndexOf("+");
            subSignTemp = inputstr.IndexOf("-");
            mulSignTemp = inputstr.IndexOf("×");
            #endregion
            while (mulSignTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, mulSignTemp);
                //MessageBox.Show(inputstr1);
                int addSignTemp1 = inputstr1.LastIndexOf("+");
                int subSignTemp1 = inputstr1.LastIndexOf("-");
                int mulSignTemp1 = inputstr1.LastIndexOf("×");
                int maxNum = maxNum_3(addSignTemp1, subSignTemp1, mulSignTemp1);
                string inputstr1_1 = "";
                string inputstr1_2 = "";
                if (maxNum == -1)
                {
                    inputstr1_1 = "";
                    inputstr1_2 = inputstr1;
                }
                else
                {
                    inputstr1_1 = inputstr1.Substring(0, maxNum + 1);
                    inputstr1_2 = inputstr1.Substring(maxNum + 1);
                }
                string inputstr2 = inputstr.Substring(mulSignTemp + 1);
                //MessageBox.Show(inputstr2);
                int addSignTemp2 = inputstr2.IndexOf("+");
                int subSignTemp2 = inputstr2.IndexOf("-");
                int mulSignTemp2 = inputstr2.LastIndexOf("×");
                int minNum_5 = minNum(addSignTemp2, subSignTemp2, mulSignTemp2, -1, -1);
                string inputstr2_1 = "";
                string inputstr2_2 = "";
                if (minNum_5 == -1)
                {
                    inputstr2_1 = inputstr2;
                    inputstr2_2 = "";
                }
                else
                {
                    inputstr2_1 = inputstr2.Substring(0, minNum_5);
                    inputstr2_2 = inputstr2.Substring(minNum_5);
                }
                //MessageBox.Show("inputstr1_1" + inputstr1_1);
                //MessageBox.Show("inputstr1_2" + inputstr1_2);
                //MessageBox.Show("inputstr2_1" + inputstr2_1);
                //MessageBox.Show("inputstr2_2" + inputstr2_2);
                string inputstr1_2_2_1 = inputstr1_2 + "×" + inputstr2_1;
                //MessageBox.Show("inputstr1_2_2_1:" + inputstr1_2_2_1);
                inputstr = inputstr1_1 + mulResult(inputstr1_2_2_1) + inputstr2_2;
                mulSignTemp = inputstr.IndexOf("×");
                //MessageBox.Show("inputstr:" + inputstr);
            }
            #endregion
            //MessageBox.Show("乘后" + inputstr);
            #region 加减运算
            inputstr = addSubResult(inputstr);
            #endregion
            return inputstr;
        }
        #endregion

        #region 加法运算
        private static string addResult(string inputstr)
        {
            double sum = 0;
            int addSignTemp = inputstr.IndexOf("+");
            while (addSignTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, addSignTemp);
                string inputstr2 = inputstr.Substring(addSignTemp + 1);
                if (inputstr1 == "")
                    sum += 0;
                else
                    sum += double.Parse(inputstr1);
                inputstr = inputstr2;
                addSignTemp = inputstr.IndexOf("+");
            }
            if (inputstr == "")
                sum += 0;
            else
                sum += double.Parse(inputstr);
            string addResult = sum.ToString();
            return addResult;
        }
        #endregion
        #region 减法运算
        private static string addSubResult(string inputstr)
        {
            string inputstring = "";
            int subSubtemp = inputstr.IndexOf("--");
            while (subSubtemp != -1)
            {
                inputstr = inputstr.Replace("--", "+");
                subSubtemp = inputstr.IndexOf("--");
            }
            //MessageBox.Show(inputstr);
            int subSignHeadTemp = inputstr.IndexOf("-");
            while (subSignHeadTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, subSignHeadTemp);
                string inputstr2 = inputstr.Substring(subSignHeadTemp + 1);
                inputstring = inputstring + inputstr1 + "+-";
                inputstr = inputstr2;
                subSignHeadTemp = inputstr.IndexOf("-");
            }
            inputstring += inputstr;
            //MessageBox.Show(inputstring);
            //inputstring = inputstr.Replace("-","+-");
            int subSignBackTemp = inputstring.LastIndexOf("-");
            while (subSignBackTemp == inputstring.Length - 1)
            {
                inputstring = inputstring.Substring(0, inputstring.Length - 1);
                subSignBackTemp = inputstring.LastIndexOf("-");
            }
            //MessageBox.Show(inputstring);
            inputstr = addResult(inputstring);
            return inputstr;
        }
        #endregion
        #region 乘法运算
        private static string mulResult(string inputstr)
        {
            string inputstr_ = inputstr;
            int subSignTemp = inputstr.IndexOf("-");
            int subNum = 0;
            while (subSignTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, subSignTemp);
                string inputstr2 = inputstr.Substring(subSignTemp + 1);
                inputstr = inputstr2;
                subSignTemp = inputstr.IndexOf("-");
                subNum++;
            }
            inputstr = inputstr_;
            //MessageBox.Show(inputstr);
            int mulSignTemp = inputstr.IndexOf("×");
            double mulSum = 1;
            while (mulSignTemp != -1)
            {
                string inputstr1 = inputstr.Substring(0, mulSignTemp);
                string inputstr2 = inputstr.Substring(mulSignTemp + 1);
                if (inputstr1 == "")
                {
                    mulSum *= 1;
                }
                else mulSum *= double.Parse(inputstr1);
                inputstr = inputstr2;
                mulSignTemp = inputstr.IndexOf("×");
            }
            if (inputstr == "")
            {
                mulSum *= 1;
            }
            else mulSum *= double.Parse(inputstr);
            //MessageBox.Show(subNum.ToString());
            //MessageBox.Show(inputstr_);
            if (subNum % 2 == 0)
            {
                if (mulSum >= 0)
                    mulSum *= 1;
                else mulSum *= -1;
            }
            else
            {
                if (mulSum <= 0)
                    mulSum *= 1;
                else mulSum *= -1;
            }
            inputstr_ = mulSum.ToString();
            return inputstr_;
        }
        #endregion
        #region 除法运算
        private static string divResult(string inputstr)
        {
            string mulDivString1 = "";
            int divSignTemp1 = inputstr.IndexOf("÷");
            while (divSignTemp1 != -1)
            {
                string inputstr1 = inputstr.Substring(0, divSignTemp1);
                string inputstr2 = inputstr.Substring(divSignTemp1 + 1);
                int divSignTemp2 = inputstr2.IndexOf("÷");
                string inputstr3 = "";
                string inputstr4 = "";
                if (divSignTemp2 != -1)
                {
                    string inputstr3_1 = "";
                    string inputstr3_2 = "";
                    inputstr3 = inputstr2.Substring(0, divSignTemp2);
                    if (inputstr3 == "0")
                        return "除数不能为0,请重新输入算式！";
                    else
                    {
                        int mulSignTemp = inputstr3.IndexOf("×");
                        if (mulSignTemp != -1)
                        {
                            inputstr3_1 = inputstr3.Substring(0, mulSignTemp);
                            inputstr3_2 = inputstr3.Substring(mulSignTemp);
                            inputstr3 = (1 / (double.Parse(inputstr3_1))).ToString() + inputstr3_2;
                        }
                        else inputstr3 = (1 / (double.Parse(inputstr3))).ToString();
                    }
                    inputstr4 = inputstr2.Substring(divSignTemp2 + 1);
                    mulDivString1 += inputstr1 + "×" + inputstr3 + "×";
                    divSignTemp1 = inputstr4.IndexOf("÷");
                    if (divSignTemp1 != -1)
                        inputstr = "1" + "÷" + inputstr4;
                    else inputstr = inputstr4;
                    //MessageBox.Show(inputstr4);
                    //MessageBox.Show(mulDivString1);
                    //MessageBox.Show(inputstr);
                }
                else
                {
                    if (inputstr2 == "")
                    {
                        mulDivString1 = "";
                        inputstr = "";
                    }
                    else
                    {
                        inputstr = inputstr2;
                        //MessageBox.Show(inputstr2);
                        mulDivString1 = inputstr1 + "×";
                        //MessageBox.Show(inputstr);
                    }
                }
                divSignTemp1 = inputstr.IndexOf("÷");
            }
            //MessageBox.Show(mulDivString1);
            //MessageBox.Show(inputstr);
            if (inputstr == "")
            {
                return "除数不能为0,请重新输入算式！";
                mulDivString1 = "0";
            }
            else
            {
                string inputstr_1 = "";
                string inputstr_2 = "";
                int mulSignTemp2 = inputstr.IndexOf("×");
                if (mulSignTemp2 != -1)
                {
                    inputstr_1 = inputstr.Substring(0, mulSignTemp2);
                    inputstr_2 = inputstr.Substring(mulSignTemp2);
                    inputstr = (1 / (double.Parse(inputstr_1))).ToString() + inputstr_2;
                }
                else inputstr = (1 / (double.Parse(inputstr))).ToString();
                mulDivString1 += inputstr;
                //MessageBox.Show(mulDivString1);
                mulDivString1 = mulResult(mulDivString1);
            }
            //MessageBox.Show(mulDivString1);
            string mulDivString2 = mulDivString1;
            int subSignTemp = mulDivString1.IndexOf("-");
            int subNum = 0;
            while (subSignTemp != -1)
            {
                string mulDivString1_1 = mulDivString1.Substring(0, subSignTemp);
                string mulDivString1_2 = mulDivString1.Substring(subSignTemp + 1);
                subNum++;
                mulDivString1 = mulDivString1_2;
                subSignTemp = mulDivString1.IndexOf("-");
            }
            //MessageBox.Show(subNum.ToString());
            if (subNum % 2 == 0)
                mulDivString1 = mulDivString2.Substring(subNum);
            else mulDivString1 = "-" + mulDivString2.Substring(subNum);
            return mulDivString1;
        }
        #endregion

        #region 取最小值
        private static int minNum(int a, int b, int c, int d, int e)
        {
            int minNum = 0;
            if (a != -1 && b != -1 && c != -1 && d != -1 && e != -1)
            {
                minNum = Math.Min(a, b);
                minNum = Math.Min(minNum, c);
                minNum = Math.Min(minNum, d);
                minNum = Math.Min(minNum, e);
            }//00000
            if (a != -1 && b != -1 && c != -1 && d != -1 && e == -1)
            {
                minNum = Math.Min(a, b);
                minNum = Math.Min(minNum, c);
                minNum = Math.Min(minNum, d);
            }//00001
            if (a != -1 && b != -1 && c != -1 && d == -1 && e != -1)
            {
                minNum = Math.Min(a, b);
                minNum = Math.Min(minNum, c);
                minNum = Math.Min(minNum, e);
            }//00010
            if (a != -1 && b != -1 && c != -1 && d == -1 && e == -1)
            {
                minNum = Math.Min(a, b);
                minNum = Math.Min(minNum, c);
            }//00011
            if (a != -1 && b != -1 && c == -1 && d != -1 && e != -1)
            {
                minNum = Math.Min(a, b);
                minNum = Math.Min(minNum, d);
                minNum = Math.Min(minNum, e);
            }//00100
            if (a != -1 && b != -1 && c == -1 && d != -1 && e == -1)
            {
                minNum = Math.Min(a, b);
                minNum = Math.Min(minNum, d);
            }//00101
            if (a != -1 && b != -1 && c == -1 && d == -1 && e != -1)
            {
                minNum = Math.Min(a, b);
                minNum = Math.Min(minNum, e);
            }//00110
            if (a != -1 && b != -1 && c == -1 && d == -1 && e == -1)
            {
                minNum = Math.Min(a, b);
            }//00111
            if (a != -1 && b == -1 && c != -1 && d != -1 && e != -1)
            {
                minNum = Math.Min(a, c);
                minNum = Math.Min(minNum, d);
                minNum = Math.Min(minNum, e);
            }//01000
            if (a != -1 && b == -1 && c != -1 && d != -1 && e == -1)
            {
                minNum = Math.Min(a, c);
                minNum = Math.Min(minNum, d);
            }//01001
            if (a != -1 && b == -1 && c != -1 && d == -1 && e == -1)
            {
                minNum = Math.Min(a, c);
                minNum = Math.Min(minNum, e);
            }//01011
            if (a != -1 && b == -1 && c == -1 && d != -1 && e != -1)
            {
                minNum = Math.Min(a, d);
                minNum = Math.Min(minNum, e);
            }//01100
            if (a != -1 && b == -1 && c == -1 && d != -1 && e == -1)
            {
                minNum = Math.Min(a, d);
            }//01101
            if (a != -1 && b == -1 && c == -1 && d == -1 && e != -1)
            {
                minNum = Math.Min(a, e);
            }//01110
            if (a != -1 && b == -1 && c == -1 && d == -1 && e == -1)
            {
                minNum = a;
            }//01111
            if (a == -1 && b != -1 && c != -1 && d != -1 && e != -1)
            {
                minNum = Math.Min(b, c);
                minNum = Math.Min(minNum, d);
                minNum = Math.Min(minNum, e);
            }//10000
            if (a == -1 && b != -1 && c != -1 && d != -1 && e == -1)
            {
                minNum = Math.Min(b, c);
                minNum = Math.Min(minNum, d);
            }//10001
            if (a == -1 && b != -1 && c != -1 && d == -1 && e != -1)
            {
                minNum = Math.Min(b, c);
                minNum = Math.Min(minNum, e);
            }//10010
            if (a == -1 && b != -1 && c != -1 && d == -1 && e == -1)
            {
                minNum = Math.Min(b, c);
            }//10011
            if (a == -1 && b != -1 && c == -1 && d != -1 && e != -1)
            {
                minNum = Math.Min(b, d);
                minNum = Math.Min(minNum, e);
            }//10100
            if (a == -1 && b != -1 && c == -1 && d != -1 && e == -1)
            {
                minNum = Math.Min(b, d);
            }//10101
            if (a == -1 && b != -1 && c == -1 && d == -1 && e != -1)
            {
                minNum = Math.Min(b, e);
            }//10110
            if (a == -1 && b != -1 && c == -1 && d == -1 && e == -1)
            {
                minNum = b;
            }//10111
            if (a == -1 && b == -1 && c != -1 && d != -1 && e != -1)
            {
                minNum = Math.Min(c, d);
                minNum = Math.Min(minNum, e);
            }//11000
            if (a == -1 && b == -1 && c != -1 && d != -1 && e == -1)
            {
                minNum = Math.Min(c, d);
            }//11001
            if (a == -1 && b == -1 && c != -1 && d == -1 && e == -1)
            {
                minNum = c;
            }//11011
            if (a == -1 && b == -1 && c == -1 && d != -1 && e != -1)
            {
                minNum = Math.Min(d, e);
            }//11100
            if (a == -1 && b == -1 && c == -1 && d != -1 && e == -1)
            {
                minNum = d;
            }//11101
            if (a == -1 && b == -1 && c == -1 && d == -1 && e != -1)
            {
                minNum = e;
            }//11110
            if (a == -1 && b == -1 && c == -1 && d == -1 && e == -1)
            {
                minNum = -1;
            }
            return minNum;
        }
        #endregion
        #region 5个数求最大值
        private static int maxNum_5(int a, int b, int c, int d, int e)
        {
            int maxNum = Math.Max(a, b);
            maxNum = Math.Max(maxNum, c);
            maxNum = Math.Max(maxNum, d);
            maxNum = Math.Max(maxNum, e);
            return maxNum;
        }
        #endregion
        #region 3个数求最大值
        private static int maxNum_3(int a, int b, int c)
        {
            int maxNum = Math.Max(a, b);
            maxNum = Math.Max(maxNum, c);
            return maxNum;
        }
        #endregion
        #region 过滤符号
        private static string filtSign(string inputstr)
        {
            int subSubSignTemp = inputstr.IndexOf("--");
            int addAddSignTemp = inputstr.IndexOf("++");
            int addSubSignTemp = inputstr.IndexOf("+-");
            int divAddSignTemp = inputstr.IndexOf("÷+");
            int mulAddSignTemp = inputstr.IndexOf("×+");
            while (subSubSignTemp != -1 || addSubSignTemp != -1 || divAddSignTemp != -1 || addAddSignTemp != -1 || mulAddSignTemp != -1)
            {
                while (subSubSignTemp != -1)
                {
                    inputstr = inputstr.Replace("--", "+");
                    subSubSignTemp = inputstr.IndexOf("--");
                }
                while (addAddSignTemp != -1)
                {
                    inputstr = inputstr.Replace("++", "+");
                    addAddSignTemp = inputstr.IndexOf("++");
                }
                while (addSubSignTemp != -1)
                {
                    inputstr = inputstr.Replace("+-", "-");
                    addSubSignTemp = inputstr.IndexOf("+-");
                }
                while (divAddSignTemp != -1)
                {
                    inputstr = inputstr.Replace("÷+", "÷");
                    divAddSignTemp = inputstr.IndexOf("÷+");
                }
                while (mulAddSignTemp != -1)
                {
                    inputstr = inputstr.Replace("×+", "×");
                    mulAddSignTemp = inputstr.IndexOf("×+");
                }
                subSubSignTemp = inputstr.IndexOf("--");
                addSubSignTemp = inputstr.IndexOf("+-");
                divAddSignTemp = inputstr.IndexOf("÷+");
                addAddSignTemp = inputstr.IndexOf("++");
                mulAddSignTemp = inputstr.IndexOf("×+");
            }
            return inputstr;
        }
        #endregion
    }
}