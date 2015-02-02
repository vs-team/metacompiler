using System.Collections.Generic;
interface IRunnable {
  IEnumerable<IRunnable> Run();
}


class _opMultiplication : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _opMultiplication(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _opAddition : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _opAddition(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _Comma : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _Comma(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}

  IEnumerable<IRunnable> Run_0_0_() {   
    var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _If; 
    if (tmp_1 != null) { 
      var c = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Then; 
      if (tmp_2 != null) { 
        var a = tmp_1.P3; var tmp_3 = tmp_1.P4 as _Else; 
        if (tmp_3 != null) { 
          var b = tmp_1.P5; 
          foreach (var tmp_4 in (new _Comma(M, c)).Run()) { 
            var c_Prime = tmp_4; 
            foreach (var tmp_5 in (new _Comma(M, new _If(c_Prime, new _Then(), a, new _Else(), b))).Run()) { 
              var res = tmp_5; 
              yield return res;  
            } 
          } 
        } 
      } 
    }
 }

  IEnumerable<IRunnable> Run_0_0_0_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _If; if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _True; if (tmp_2 != null) { var tmp_3 = tmp_1.P2 as _Then; if (tmp_3 != null) { var a = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; if (tmp_4 != null) { var b = tmp_1.P5; foreach (var tmp_5 in (new _Comma(M, a)).Run()) { var res = tmp_5; yield return res;  } } } } }
 }
  IEnumerable<IRunnable> Run_0_1_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as rule; if (tmp_1 != null) { var FS = tmp_1.P1; var tmp_2 = tmp_1.P2 as _DefinedAs; if (tmp_2 != null) { var tmp_3 = tmp_1.P3 as _Comma; if (tmp_3 != null) { var b = tmp_3.P1; var b0 = tmp_3.P2; foreach (var tmp_4 in (new _Comma(M, b)).Run()) { var tmp_5 = tmp_4 as _Comma; if (tmp_5 != null) { var M_Prime = tmp_5.P1; var tmp_6 = tmp_5.P2 as unit; if (tmp_6 != null) { yield return new _Comma(M_Prime, new rule(FS, new _DefinedAs(), new _Comma(b0, b0)));  } } } } } }
 }
  IEnumerable<IRunnable> Run_0_2_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as unit; if (tmp_1 != null) { yield return M;  }
 }
  IEnumerable<IRunnable> Run_1_0_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Semicolon; if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; foreach (var tmp_2 in (new _Comma(M, a)).Run()) { var tmp_3 = tmp_2 as unit; if (tmp_3 != null) { foreach (var tmp_4 in (new _Comma(M, b)).Run()) { var res = tmp_4; yield return res;  } } } }
 }
  IEnumerable<IRunnable> Run_1_0_0_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _If; if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _False; if (tmp_2 != null) { var tmp_3 = tmp_1.P2 as _Then; if (tmp_3 != null) { var a = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; if (tmp_4 != null) { var b = tmp_1.P5; foreach (var tmp_5 in (new _Comma(M, b)).Run()) { var res = tmp_5; yield return res;  } } } } }
 }
  IEnumerable<IRunnable> Run_1_1_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as rule; if (tmp_1 != null) { var FS = tmp_1.P1; var tmp_2 = tmp_1.P2 as _DefinedAs; if (tmp_2 != null) { var tmp_3 = tmp_1.P3 as _Comma; if (tmp_3 != null) { var b = tmp_3.P1; var b0 = tmp_3.P2; foreach (var tmp_4 in (new _Comma(M, b)).Run()) { var tmp_5 = tmp_4 as _Comma; if (tmp_5 != null) { var M_Prime = tmp_5.P1; var tmp_6 = tmp_5.P2 as cont; if (tmp_6 != null) { var tmp_7 = tmp_6.P1 as _Semicolon; if (tmp_7 != null) { var tmp_8 = tmp_7.P1 as yield; if (tmp_8 != null) { var res = tmp_8.P1; var b_Prime = tmp_7.P2; foreach (var tmp_9 in (new assignFields(M_Prime, FS, res)).Run()) { var M_Prime_Prime = tmp_9; yield return new _Comma(M_Prime_Prime, new rule(FS, new _DefinedAs(), new _Comma(b_Prime, b0)));  } } } } } } } } }
 }
  IEnumerable<IRunnable> Run_2_0_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Semicolon; if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; foreach (var tmp_2 in (new _Comma(M, a)).Run()) { var tmp_3 = tmp_2 as _Comma; if (tmp_3 != null) { var M_Prime = tmp_3.P1; var tmp_4 = tmp_3.P2 as cont; if (tmp_4 != null) { var a_Prime = tmp_4.P1; yield return new _Comma(M_Prime, new cont(new _Semicolon(a_Prime, b)));  } } } }
 }
  IEnumerable<IRunnable> Run_2_1_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as rule; if (tmp_1 != null) { var FS = tmp_1.P1; var tmp_2 = tmp_1.P2 as _DefinedAs; if (tmp_2 != null) { var tmp_3 = tmp_1.P3 as _Comma; if (tmp_3 != null) { var b = tmp_3.P1; var b0 = tmp_3.P2; foreach (var tmp_4 in (new _Comma(M, b)).Run()) { var tmp_5 = tmp_4 as _Comma; if (tmp_5 != null) { var M_Prime = tmp_5.P1; var tmp_6 = tmp_5.P2 as cont; if (tmp_6 != null) { var tmp_7 = tmp_6.P1 as _Semicolon; if (tmp_7 != null) { var tmp_8 = tmp_7.P1 as wait; if (tmp_8 != null) { var c = tmp_8.P1; var b_Prime = tmp_7.P2; yield return new _Comma(M_Prime, new rule(FS, new _DefinedAs(), new _Comma(b_Prime, b0)));  } } } } } } } }
 }
  IEnumerable<IRunnable> Run_3_0_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as wait; if (tmp_1 != null) { var c = tmp_1.P1; foreach (var tmp_2 in (new _Comma(M, c)).Run()) { var tmp_3 = tmp_2 as _Comma; if (tmp_3 != null) { var M_Prime = tmp_3.P1; var tmp_4 = tmp_3.P2 as _True; if (tmp_4 != null) { yield return new _Comma(M_Prime, new unit());  } } } }
 }
  IEnumerable<IRunnable> Run_4_0_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as wait; if (tmp_1 != null) { var c = tmp_1.P1; foreach (var tmp_2 in (new _Comma(M, c)).Run()) { var tmp_3 = tmp_2 as _Comma; if (tmp_3 != null) { var M_Prime = tmp_3.P1; var tmp_4 = tmp_3.P2 as _False; if (tmp_4 != null) { yield return new _Comma(M_Prime, new cont(new _Semicolon(new wait(c), new unit())));  } } } }
 }
  IEnumerable<IRunnable> Run_5_0_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as yield; if (tmp_1 != null) { var a = tmp_1.P1; foreach (var tmp_2 in (new _Comma(M, a)).Run()) { var tmp_3 = tmp_2 as _Comma; if (tmp_3 != null) { var M_Prime = tmp_3.P1; var res = tmp_3.P2; yield return new _Comma(M_Prime, new cont(new _Semicolon(new yield(res), new unit())));  } } }
 }

public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _opSubtraction : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _opSubtraction(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _Arrow : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _Arrow(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _opDivision : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _opDivision(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _DefinedAs : IRunnable {

public _DefinedAs() {}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _Semicolon : IRunnable {
public IRunnable P1;
public IRunnable P2;

public _Semicolon(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}

  IEnumerable<IRunnable> Run_1_2_() {   var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Comma; if (tmp_1 != null) { var M = tmp_1.P1; var r = tmp_1.P2; var rs = tmp_0.P2; foreach (var tmp_2 in (new ruleEval(M, r)).Run()) { var tmp_3 = tmp_2 as _Comma; if (tmp_3 != null) { var M_Prime = tmp_3.P1; var r_Prime = tmp_3.P2; foreach (var tmp_4 in (new rulesEval(M_Prime, rs)).Run()) { var tmp_5 = tmp_4 as _Comma; if (tmp_5 != null) { var M_Prime_Prime = tmp_5.P1; var rs_Prime = tmp_5.P2; yield return new _Semicolon(new _Comma(M_Prime_Prime, r_Prime), rs_Prime);  } } } } }
 }

public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class arith_op : IRunnable {
public IRunnable P1;
public IRunnable P2;
public IRunnable P3;

public arith_op(IRunnable P1, IRunnable P2, IRunnable P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class assignFields : IRunnable {
public IRunnable P1;
public IRunnable P2;
public IRunnable P3;

public assignFields(IRunnable P1, IRunnable P2, IRunnable P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}

  IEnumerable<IRunnable> Run_3_1_() {   var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as unit; if (tmp_1 != null) { var res = tmp_0.P3; yield return M;  }
 }

public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class bool_op : IRunnable {
public IRunnable P1;
public IRunnable P2;
public IRunnable P3;

public bool_op(IRunnable P1, IRunnable P2, IRunnable P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class cont : IRunnable {
public IRunnable P1;

public cont(IRunnable P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _Else : IRunnable {

public _Else() {}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class exprEval : IRunnable {
public IRunnable P1;
public IRunnable P2;

public exprEval(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}

  IEnumerable<IRunnable> Run_0_() {   var tmp_0 = this; var M = tmp_0.P1; var expr = tmp_0.P2; foreach (var tmp_1 in (new _Comma(M, expr)).Run()) { var res = tmp_1; yield return res;  }
 }

public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _False : IRunnable {

public _False() {}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class id : IRunnable {
public IRunnable P1;

public id(IRunnable P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _If : IRunnable {
public IRunnable P1;
public IRunnable P2;
public IRunnable P3;
public IRunnable P4;
public IRunnable P5;

public _If(IRunnable P1, IRunnable P2, IRunnable P3, IRunnable P4, IRunnable P5) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class rule : IRunnable {
public IRunnable P1;
public IRunnable P2;
public IRunnable P3;

public rule(IRunnable P1, IRunnable P2, IRunnable P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class ruleEval : IRunnable {
public IRunnable P1;
public IRunnable P2;

public ruleEval(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}

  IEnumerable<IRunnable> Run_1_() {   var tmp_0 = this; var M = tmp_0.P1; var r = tmp_0.P2; foreach (var tmp_1 in (new _Comma(M, r)).Run()) { var tmp_2 = tmp_1 as _Comma; if (tmp_2 != null) { var M_Prime = tmp_2.P1; var r_Prime = tmp_2.P2; yield return new _Comma(M_Prime, r_Prime);  } }
 }

public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class rulesEval : IRunnable {
public IRunnable P1;
public IRunnable P2;

public rulesEval(IRunnable P1, IRunnable P2) {this.P1 = P1; this.P2 = P2;}

  IEnumerable<IRunnable> Run_2_() {   var tmp_0 = this; var M = tmp_0.P1; var rs = tmp_0.P2; foreach (var tmp_1 in (new _Comma(M, rs)).Run()) { var tmp_2 = tmp_1 as _Comma; if (tmp_2 != null) { var M_Prime = tmp_2.P1; var rs_Prime = tmp_2.P2; yield return new _Comma(M_Prime, rs_Prime);  } }
 }

public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _Then : IRunnable {

public _Then() {}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class _True : IRunnable {

public _True() {}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class unit : IRunnable {

public unit() {}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class wait : IRunnable {
public IRunnable P1;

public wait(IRunnable P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

class yield : IRunnable {
public IRunnable P1;

public yield(IRunnable P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { if (false) yield return null; }
}

