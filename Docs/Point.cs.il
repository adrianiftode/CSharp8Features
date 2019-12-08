// Type: CSharpFeatures.CSharp8FeaturesDemo 
// Assembly: CSharpFeatures, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7C473163-C8E1-4E39-ACC7-196DF63E3161
// Location: E:\projects\mine\CSharp8.0\CSharp8Features\bin\Release\netcoreapp3.1\CSharpFeatures.dll
// Sequence point data from E:\projects\mine\CSharp8.0\CSharp8Features\bin\Release\netcoreapp3.1\CSharpFeatures.pdb

.class public auto ansi beforefieldinit
  CSharpFeatures.CSharp8FeaturesDemo
    extends [System.Runtime]System.Object
{

  .class nested public sealed sequential ansi beforefieldinit
    Point
      extends [System.Runtime]System.ValueType
  {

    .field private float64 '<X>k__BackingField'
    .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
      = (01 00 00 00 )

    .field private float64 '<Y>k__BackingField'
    .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
      = (01 00 00 00 )

    .method public hidebysig specialname instance float64
      get_X() cil managed
    {
      .custom instance void [System.Runtime]System.Runtime.CompilerServices.IsReadOnlyAttribute::.ctor()
        = (01 00 00 00 )
      .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
        = (01 00 00 00 )
      .maxstack 8

      // [10 31 - 10 35]
      IL_0000: ldarg.0      // this
      IL_0001: ldfld        float64 CSharpFeatures.CSharp8FeaturesDemo/Point::'<X>k__BackingField'
      IL_0006: ret

    } // end of method Point::get_X

    .method public hidebysig specialname instance void
      set_X(
        float64 'value'
      ) cil managed
    {
      .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
        = (01 00 00 00 )
      .maxstack 8

      // [10 36 - 10 40]
      IL_0000: ldarg.0      // this
      IL_0001: ldarg.1      // 'value'
      IL_0002: stfld        float64 CSharpFeatures.CSharp8FeaturesDemo/Point::'<X>k__BackingField'
      IL_0007: ret

    } // end of method Point::set_X

    .method public hidebysig specialname instance float64
      get_Y() cil managed
    {
      .custom instance void [System.Runtime]System.Runtime.CompilerServices.IsReadOnlyAttribute::.ctor()
        = (01 00 00 00 )
      .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
        = (01 00 00 00 )
      .maxstack 8

      // [11 31 - 11 35]
      IL_0000: ldarg.0      // this
      IL_0001: ldfld        float64 CSharpFeatures.CSharp8FeaturesDemo/Point::'<Y>k__BackingField'
      IL_0006: ret

    } // end of method Point::get_Y

    .method public hidebysig specialname instance void
      set_Y(
        float64 'value'
      ) cil managed
    {
      .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
        = (01 00 00 00 )
      .maxstack 8

      // [11 36 - 11 40]
      IL_0000: ldarg.0      // this
      IL_0001: ldarg.1      // 'value'
      IL_0002: stfld        float64 CSharpFeatures.CSharp8FeaturesDemo/Point::'<Y>k__BackingField'
      IL_0007: ret

    } // end of method Point::set_Y

    .method public hidebysig specialname instance float64
      get_Distance() cil managed
    {
      .maxstack 8

      // [12 39 - 12 63]
      IL_0000: ldarg.0      // this
      IL_0001: call         instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_X()
      IL_0006: ldarg.0      // this
      IL_0007: call         instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_X()
      IL_000c: mul
      IL_000d: ldarg.0      // this
      IL_000e: call         instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_Y()
      IL_0013: ldarg.0      // this
      IL_0014: call         instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_Y()
      IL_0019: mul
      IL_001a: add
      IL_001b: call         float64 [System.Runtime.Extensions]System.Math::Sqrt(float64)
      IL_0020: ret

    } // end of method Point::get_Distance

    .method public hidebysig virtual instance string
      ToString() cil managed
    {
      .custom instance void [System.Runtime]System.Runtime.CompilerServices.IsReadOnlyAttribute::.ctor()
        = (01 00 00 00 )
      .maxstack 4
      .locals init (
        [0] valuetype CSharpFeatures.CSharp8FeaturesDemo/Point V_0
      )

      // [13 59 - 13 102]
      IL_0000: ldstr        "({0}, {1}) is {2} from the origin"
      IL_0005: ldarg.0      // this
      IL_0006: call         instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_X()
      IL_000b: box          [System.Runtime]System.Double
      IL_0010: ldarg.0      // this
      IL_0011: call         instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_Y()
      IL_0016: box          [System.Runtime]System.Double
      IL_001b: ldarg.0      // this
      IL_001c: ldobj        CSharpFeatures.CSharp8FeaturesDemo/Point
      IL_0021: stloc.0      // V_0
      IL_0022: ldloca.s     V_0
      IL_0024: call         instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_Distance()
      IL_0029: box          [System.Runtime]System.Double
      IL_002e: call         string [System.Runtime]System.String::Format(string, object, object, object)
      IL_0033: ret

    } // end of method Point::ToString

    .property instance float64 X()
    {
      .get instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_X()
      .set instance void CSharpFeatures.CSharp8FeaturesDemo/Point::set_X(float64)
    } // end of property Point::X

    .property instance float64 Y()
    {
      .get instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_Y()
      .set instance void CSharpFeatures.CSharp8FeaturesDemo/Point::set_Y(float64)
    } // end of property Point::Y

    .property instance float64 Distance()
    {
      .get instance float64 CSharpFeatures.CSharp8FeaturesDemo/Point::get_Distance()
    } // end of property Point::Distance
  } // end of class Point

  .method public hidebysig specialname rtspecialname instance void
    .ctor() cil managed
  {
    .maxstack 8

    IL_0000: ldarg.0      // this
    IL_0001: call         instance void [System.Runtime]System.Object::.ctor()
    IL_0006: ret

  } // end of method CSharp8FeaturesDemo::.ctor
} // end of class CSharpFeatures.CSharp8FeaturesDemo
