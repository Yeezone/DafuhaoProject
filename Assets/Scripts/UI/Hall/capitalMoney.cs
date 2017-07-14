using UnityEngine;
using System.Collections;

public class capitalMoney : MonoBehaviour {

	public	double			maxCount;

	public	GameObject		money_input;
	public	GameObject		capital_label;

	private const string DXSZ = "零壹贰叁肆伍陆柒捌玖";
	private const string DXDW = " 拾佰仟万拾佰仟亿拾佰仟万";
	private const string SCDW = " 拾佰仟万亿";



	public void OnMoneyChange () {
		if(capital_label != null) capital_label.GetComponent<UILabel>().text = "";


		string capValue = money_input.GetComponent<UIInput>().value;
		string afterDot = "";	//小数点后字符
		string capAfterDot = "";
		string currCap = "";	//当前金额
		string capResult = "";	//结果金额
		string currUnit = "";	//当前单位
		string resultUnit = "";	//结果单位
		int prevChar = -1;		//上一位的值
		int currChar = 0;		//当前的值
		int posIndex = 0;		//位置索引

		if(capValue.Length == 0) return;
		
		//首位去除"-"号
		if(capValue.Substring(0,1) == "-")
		{
			money_input.GetComponent<UIInput>().value = capValue = capValue.Substring(1);
		}
		//超过最大值时 按最大值计算
		if(double.Parse(capValue) > maxCount) money_input.GetComponent<UIInput>().value = capValue = maxCount.ToString();

		//分隔小数
		for(int i = 0; i < capValue.Length; i++)
		{
			if(capValue.Substring(i,1) == ".")
			{
				afterDot = capValue.Substring(i+1);
				capValue = capValue.Substring(0,i);
				break;
			}
		}

		for(int i = capValue.Length - 1; i >= 0; i--)
		{
			currChar = int.Parse(capValue.Substring(i,1));
			if(posIndex > 12)
			{
				//超出最大精度"万亿"
				break;
			}else if(currChar != 0)
			{
				//当前位为非零值,直接转换成大写金额
				currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex,1);
			}else{
				//防止转换后出现多余的零,例如:3000020
				switch (posIndex)
				{
					case 0:
						currCap = "";
						break;
					case 4:
						currCap = "万";
						break;
					case 8:
						currCap = "亿";
						break;
					default:
						break;
				}
				if(prevChar != 0)
				{
					if(currCap != "")
					{
						if(currCap != " ") currCap += "零";
					}else{
						currCap = "零";
					}
				}
			}
			//对结果进行容错处理
			if(capResult.Length > 0)
			{
				resultUnit = capResult.Substring(0,1);
				currUnit = DXDW.Substring(posIndex,1);
				if(SCDW.IndexOf(resultUnit) > 0)
				{
					if(SCDW.IndexOf(currUnit) > SCDW.IndexOf(resultUnit))
					{
						capResult = capResult.Substring(1);
					}
				}
			}
			capResult = currCap + capResult;
			prevChar = currChar;
			posIndex += 1;
			currCap = "";
		}
		if(capResult.Substring(capResult.Length-1) == "零") capResult = capResult.Substring(0,capResult.Length-1);
		if(afterDot != "")
		{
			capAfterDot = "点";
			for(int i = 0; i < afterDot.Length; i++)
			{
				capAfterDot += DXSZ.Substring( int.Parse(afterDot.Substring(i,1)), 1 );
			}
		}
		string tempValue = capResult + capAfterDot;
		string finalValue = "";
		for(int i = 0; i < tempValue.Length; i++)
		{
			if(tempValue.Substring(i,1) == " ")
			{
				continue;
			}
			finalValue += tempValue.Substring(i,1);
		}
		if(capital_label != null) capital_label.GetComponent<UILabel>().text = finalValue;
	}
}
