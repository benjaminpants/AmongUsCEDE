using System;
using System.Collections.Generic;
using System.Text;
using MoonSharp.Interpreter;

namespace AmongUsCEDE.LuaReimplments
{
	public class MathReimplement
	{
		public void abs(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			number = DynValue.NewNumber(Math.Abs(number.Number));
		}

		public void acos(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			number = DynValue.NewNumber(Math.Acos(number.Number));
		}

		public void asin(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			number = DynValue.NewNumber(Math.Asin(number.Number));
		}

		public void atan(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			number = DynValue.NewNumber(Math.Atan(number.Number));
		}

		public void atan2(ref DynValue number, ref DynValue number2)
		{
			if (number.Type != DataType.Number || number2.Type != DataType.Number) throw new InvalidCastException();
			number = DynValue.NewNumber(Math.Atan2(number.Number, number2.Number));
		}

		public void ceil(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			number = DynValue.NewNumber(Math.Ceiling(number.Number));
		}

		public void cos(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			number = DynValue.NewNumber(Math.Cos(number.Number));
		}

		public void cosh(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			number = DynValue.NewNumber(Math.Cosh(number.Number));
		}

		public void deg(ref DynValue number)
		{
			throw new NotImplementedException();
		}
	}
}
