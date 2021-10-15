using System;
using System.Collections.Generic;
using System.Text;
using MoonSharp.Interpreter;

namespace AmongUsCEDE.LuaReimplments
{
	public class MathReimplement
	{
		public DynValue abs(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Abs(number.Number));
		}

		public DynValue acos(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Acos(number.Number));
		}

		public DynValue asin(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Asin(number.Number));
		}

		public DynValue atan(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Atan(number.Number));
		}

		public DynValue atan2(ref DynValue number, ref DynValue number2)
		{
			if (number.Type != DataType.Number || number2.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Atan2(number.Number, number2.Number));
		}

		public DynValue ceil(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Ceiling(number.Number));
		}

		public DynValue cos(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Cos(number.Number));
		}

		public DynValue cosh(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Cosh(number.Number));
		}

		public DynValue deg(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber((180 / Math.PI) * number.Number);
		}

		public DynValue exp(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Exp(number.Number));
		}

		public DynValue floor(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Floor(number.Number));
		}

		public DynValue fmod(ref DynValue number, ref DynValue number2)
		{
			throw new NotImplementedException();
		}

		public DynValue frexp(ref DynValue number)
		{
			throw new NotImplementedException();
		}

		public double huge()
		{
			throw new NotImplementedException();
		}

		public double Idexp(ref DynValue number, ref DynValue number2)
		{
			throw new NotImplementedException();
		}

		public DynValue log(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Log(number.Number));
		}

		public DynValue log10(ref DynValue number)
		{
			if (number.Type != DataType.Number) throw new InvalidCastException();
			return DynValue.NewNumber(Math.Log10(number.Number));
		}




	}
}
