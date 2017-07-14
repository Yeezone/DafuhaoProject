using System;

[Flags]
public enum ButtonStyle
{
	Yes = 0x1,
	No = 0x2,
	OK = 0x4,
	Cancel = 0x8,
	Confirm = 0x10
}