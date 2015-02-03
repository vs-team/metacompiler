using System.Collections.Generic;
using System.Linq;
namespace PeanoNumbers {
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public class _opMultiplication : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _opMultiplication(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   {
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
if (tmp_1 != null) { var a = tmp_0.P2; 
yield return new z();  }
}
  {
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
foreach (var tmp_2 in (a).Run()) { var a_Prime = tmp_2; 
foreach (var tmp_3 in (b).Run()) { var b_Prime = tmp_3; 
foreach (var tmp_4 in (new _opMultiplication(a_Prime, b_Prime)).Run()) { var c = tmp_4; 
foreach (var tmp_5 in (c).Run()) { var c_Prime = tmp_5; 
foreach (var tmp_6 in (new _opAddition(c_Prime, b_Prime)).Run()) { var d = tmp_6; 
yield return d;  } } } } } }
}
  }


public override string ToString() { var res = "*" + "(";
res += P1.ToString();
res += P2.ToString();
;
res += ")";
return res;
}
}

public class _opAddition : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _opAddition(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   {
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
if (tmp_1 != null) { var a = tmp_0.P2; 
foreach (var tmp_2 in (a).Run()) { var a_Prime = tmp_2; 
yield return a_Prime;  } }
}
  {
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
foreach (var tmp_2 in (a).Run()) { var a_Prime = tmp_2; 
foreach (var tmp_3 in (b).Run()) { var b_Prime = tmp_3; 
foreach (var tmp_4 in (new _opAddition(a_Prime, b_Prime)).Run()) { var c = tmp_4; 
yield return new s(c);  } } } }
}
  }


public override string ToString() { var res = "+" + "(";
res += P1.ToString();
res += P2.ToString();
;
res += ")";
return res;
}
}

public class s : IRunnable {
public IRunnable P1;

public s(IRunnable P1) {this.P1 = P1;}

  public IEnumerable<IRunnable> Run() {   {
var tmp_0 = this; var a = tmp_0.P1; 
foreach (var tmp_1 in (a).Run()) { var a_Prime = tmp_1; 
yield return new s(a_Prime);  }
}
  }


public override string ToString() { var res = "s" + "(";
res += P1.ToString();
;
res += ")";
return res;
}
}

public class z : IRunnable {

public z() {}

  public IEnumerable<IRunnable> Run() {   {
var tmp_0 = this as z; 
yield return new z(); 
}
  }


public override string ToString() { var res = "z";
return res;
}
}




public class EntryPoint : IRunnable {
 public IEnumerable<IRunnable> Run()
{
foreach(var x in new _opMultiplication(new s(new s(new z())), new s(new s(new z()))).Run())
yield return x;
}
}

}
