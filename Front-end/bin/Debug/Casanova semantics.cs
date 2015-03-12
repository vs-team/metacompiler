using System.Collections.Generic;
using System.Linq;
namespace Casanova_semantics
{
  public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
  public interface IRunnable
  {
    IEnumerable<IRunnable> Run();
    IEnumerable<IRunnable> Run3_();
    IEnumerable<IRunnable> Run3_6_();
    IEnumerable<IRunnable> Run3_23_();
    IEnumerable<IRunnable> Run3_24_();
  }


  public interface BoolConst : BoolExpr, Expr, ExprResult { }
  public interface BoolExpr : Expr { }
  public interface Domain : IRunnable { }
  public interface Else : IRunnable { }
  public interface Entity : IRunnable { }
  public interface EntityManager : IRunnable { }
  public interface Expr : IRunnable { }
  public interface ExprList : Expr { }
  public interface ExprResult : Expr { }
  public interface ExprResultList : IRunnable { }
  public interface Id : Expr { }
  public interface IntConst : Expr, ExprResult, IntExp { }
  public interface IntExp : Expr { }
  public interface Locals : IRunnable { }
  public interface MemoryOp : IRunnable { }
  public interface Rule : IRunnable { }
  public interface RuleEvaluation : IRunnable { }
  public interface RuleList : IRunnable { }
  public interface RuleManager : IRunnable { }
  public interface RuleResult : IRunnable { }
  public interface Test : IRunnable { }
  public interface Then : IRunnable { }
  public interface UpdateResult : IRunnable { }



  public class _opDollar : Id
  {
    public string P1;

    public _opDollar(string P1) { this.P1 = P1; }
    public static _opDollar Create(string P1) { return new _opDollar(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " $ "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach (var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}"; } else { res += P1.ToString(); }

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opDollar;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opDollarb : BoolConst
  {
    public bool P1;

    public _opDollarb(bool P1) { this.P1 = P1; }
    public static _opDollarb Create(bool P1) { return new _opDollarb(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " $b "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opDollarb;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opDollari : IntConst
  {
    public int P1;

    public _opDollari(int P1) { this.P1 = P1; }
    public static _opDollari Create(int P1) { return new _opDollari(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " $i "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opDollari;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opDollarm : Locals
  {
    public System.Collections.Immutable.ImmutableDictionary<string, ExprResult> P1;

    public _opDollarm(System.Collections.Immutable.ImmutableDictionary<string, ExprResult> P1) { this.P1 = P1; }
    public static _opDollarm Create(System.Collections.Immutable.ImmutableDictionary<string, ExprResult> P1) { return new _opDollarm(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " $m "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach (var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}"; } else { res += P1.ToString(); }

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opDollarm;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opAnd : BoolExpr
  {
    public BoolExpr P1;
    public BoolExpr P2;

    public _opAnd(BoolExpr P1, BoolExpr P2) { this.P1 = P1; this.P2 = P2; }
    public static _opAnd Create(BoolExpr P1, BoolExpr P2) { return new _opAnd(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " && "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opAnd;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opMultiplication : IntExp
  {
    public IntExp P1;
    public IntExp P2;

    public _opMultiplication(IntExp P1, IntExp P2) { this.P1 = P1; this.P2 = P2; }
    public static _opMultiplication Create(IntExp P1, IntExp P2) { return new _opMultiplication(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " * "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opMultiplication;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opAddition : IntExp
  {
    public IntExp P1;
    public IntExp P2;

    public _opAddition(IntExp P1, IntExp P2) { this.P1 = P1; this.P2 = P2; }
    public static _opAddition Create(IntExp P1, IntExp P2) { return new _opAddition(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " + "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opAddition;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opSubtraction : IntExp
  {
    public IntExp P1;
    public IntExp P2;

    public _opSubtraction(IntExp P1, IntExp P2) { this.P1 = P1; this.P2 = P2; }
    public static _opSubtraction Create(IntExp P1, IntExp P2) { return new _opSubtraction(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " - "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opSubtraction;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opDivision : IntExp
  {
    public IntExp P1;
    public IntExp P2;

    public _opDivision(IntExp P1, IntExp P2) { this.P1 = P1; this.P2 = P2; }
    public static _opDivision Create(IntExp P1, IntExp P2) { return new _opDivision(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " / "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opDivision;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _Semicolon : ExprList
  {
    public Expr P1;
    public ExprList P2;

    public _Semicolon(Expr P1, ExprList P2) { this.P1 = P1; this.P2 = P2; }
    public static _Semicolon Create(Expr P1, ExprList P2) { return new _Semicolon(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " ; "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _Semicolon;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opEquals : BoolExpr
  {
    public IntExp P1;
    public IntExp P2;

    public _opEquals(IntExp P1, IntExp P2) { this.P1 = P1; this.P2 = P2; }
    public static _opEquals Create(IntExp P1, IntExp P2) { return new _opEquals(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " = "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opEquals;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opGreaterThan : BoolExpr
  {
    public IntExp P1;
    public IntExp P2;

    public _opGreaterThan(IntExp P1, IntExp P2) { this.P1 = P1; this.P2 = P2; }
    public static _opGreaterThan Create(IntExp P1, IntExp P2) { return new _opGreaterThan(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " > "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opGreaterThan;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class add : Locals
  {
    public Locals P1;
    public string P2;
    public ExprResult P3;

    public add(Locals P1, string P2, ExprResult P3) { this.P1 = P1; this.P2 = P2; this.P3 = P3; }
    public static add Create(Locals P1, string P2, ExprResult P3) { return new add(P1, P2, P3); }

    public IEnumerable<IRunnable> Run()
    {
      {
#line 91 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm;
#line 91 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3;
#line 91 "Content\Casanova semantics\transform.mc"
          if ((M.ContainsKey(k)).Equals(false))
          {
            var M_Prime = (M.Add(k, v));
#line 91 "Content\Casanova semantics\transform.mc"
            var result = _opDollarm.Create(M_Prime);
#line 91 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }


      {
#line 96 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm;
#line 96 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3;
#line 96 "Content\Casanova semantics\transform.mc"
          if ((M.ContainsKey(k)).Equals(true))
          {
            var M_Prime = (M.SetItem(k, v));
#line 96 "Content\Casanova semantics\transform.mc"
            var result = _opDollarm.Create(M_Prime);
#line 96 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }

    }

    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " add "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach (var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}"; } else { res += P2.ToString(); }
      if (P3 is System.Collections.IEnumerable) { res += "{"; foreach (var x in P3 as System.Collections.IEnumerable) res += x.ToString(); res += "}"; } else { res += P3.ToString(); }

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as add;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class atomic : ExprResult
  {

    public atomic() { }
    public static atomic Create() { return new atomic(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "atomic";
    }

    public override bool Equals(object other)
    {
      return other is atomic;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class binding : ExprResult
  {
    public Id P1;
    public ExprResult P2;
    public ExprList P3;

    public binding(Id P1, ExprResult P2, ExprList P3) { this.P1 = P1; this.P2 = P2; this.P3 = P3; }
    public static binding Create(Id P1, ExprResult P2, ExprList P3) { return new binding(P1, P2, P3); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " binding "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as binding;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class consDomain : Domain
  {
    public string P1;
    public Domain P2;

    public consDomain(string P1, Domain P2) { this.P1 = P1; this.P2 = P2; }
    public static consDomain Create(string P1, Domain P2) { return new consDomain(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      if (P1 is System.Collections.IEnumerable) { res += "{"; foreach (var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}"; } else { res += P1.ToString(); }

      res += " consDomain "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as consDomain;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class consResult : ExprResultList
  {
    public ExprResult P1;
    public ExprResultList P2;

    public consResult(ExprResult P1, ExprResultList P2) { this.P1 = P1; this.P2 = P2; }
    public static consResult Create(ExprResult P1, ExprResultList P2) { return new consResult(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " consResult "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as consResult;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class consRule : RuleList
  {
    public Rule P1;
    public RuleList P2;

    public consRule(Rule P1, RuleList P2) { this.P1 = P1; this.P2 = P2; }
    public static consRule Create(Rule P1, RuleList P2) { return new consRule(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " consRule "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as consRule;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class continueWith : ExprResult
  {
    public ExprList P1;

    public continueWith(ExprList P1) { this.P1 = P1; }
    public static continueWith Create(ExprList P1) { return new continueWith(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " continueWith "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as continueWith;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _Else : Else
  {

    public _Else() { }
    public static _Else Create() { return new _Else(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "else";
    }

    public override bool Equals(object other)
    {
      return other is _Else;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class entity : Entity
  {
    public string P1;
    public Locals P2;
    public RuleList P3;

    public entity(string P1, Locals P2, RuleList P3) { this.P1 = P1; this.P2 = P2; this.P3 = P3; }
    public static entity Create(string P1, Locals P2, RuleList P3) { return new entity(P1, P2, P3); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " entity "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach (var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}"; } else { res += P1.ToString(); }
      res += P2.ToString();
      res += P3.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as entity;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class eval : Expr
  {
    public float P1;
    public Locals P2;
    public Locals P3;
    public Expr P4;

    public eval(float P1, Locals P2, Locals P3, Expr P4) { this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; }
    public static eval Create(float P1, Locals P2, Locals P3, Expr P4) { return new eval(P1, P2, P3, P4); }

    public IEnumerable<IRunnable> Run3_()
    {
      {
#line 189 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as let;
#line 189 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var id = tmp_1.P1; var exp = tmp_1.P2;
#line 189 "Content\Casanova semantics\transform.mc"
          if (M is Locals && lv is Locals && exp is Expr)
          {
#line 189 "Content\Casanova semantics\transform.mc"
            var tmp_3 = eval.Create(dt, M as Locals, lv as Locals, exp as Expr);
#line 189 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_())
            {
              var exp_Prime = tmp_2;
#line 189 "Content\Casanova semantics\transform.mc"
              if (id is Id && exp_Prime is ExprResult)
              {
#line 189 "Content\Casanova semantics\transform.mc"
                var result = letResult.Create(id as Id, exp_Prime as ExprResult);
#line 189 "Content\Casanova semantics\transform.mc"
                yield return result;
              }
            }
          }
        }
      }


      {
#line 193 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _If;
#line 193 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var c = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Then;
#line 193 "Content\Casanova semantics\transform.mc"
          if (tmp_2 != null)
          {
            var t = tmp_1.P3; var tmp_3 = tmp_1.P4 as _Else;
#line 193 "Content\Casanova semantics\transform.mc"
            if (tmp_3 != null)
            {
              var e = tmp_1.P5;
#line 193 "Content\Casanova semantics\transform.mc"
              if (!c.Equals(_opDollarb.Create(true)))
              {
#line 193 "Content\Casanova semantics\transform.mc"
                if (!c.Equals(_opDollarb.Create(false)))
                {
#line 193 "Content\Casanova semantics\transform.mc"
                  if (M is Locals && lv is Locals && c is Expr)
                  {
#line 193 "Content\Casanova semantics\transform.mc"
                    var tmp_5 = eval.Create(dt, M as Locals, lv as Locals, c as Expr);
#line 193 "Content\Casanova semantics\transform.mc"
                    foreach (var tmp_4 in tmp_5.Run3_())
                    {
                      var c_Prime = tmp_4;
#line 193 "Content\Casanova semantics\transform.mc"
                      if (M is Locals && lv is Locals && c_Prime is BoolExpr && t is ExprList && e is ExprList)
                      {
#line 193 "Content\Casanova semantics\transform.mc"
                        var tmp_7 = eval.Create(dt, M as Locals, lv as Locals, _If.Create(c_Prime as BoolExpr, _Then.Create(), t as ExprList, _Else.Create(), e as ExprList));
#line 193 "Content\Casanova semantics\transform.mc"
                        foreach (var tmp_6 in tmp_7.Run3_())
                        {
                          var res = tmp_6;
#line 193 "Content\Casanova semantics\transform.mc"
                          var result = res;
#line 193 "Content\Casanova semantics\transform.mc"
                          yield return result;
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 200 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _If;
#line 200 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var tmp_2 = tmp_1.P1 as _opDollarb;
#line 200 "Content\Casanova semantics\transform.mc"
          if (tmp_2 != null)
          {
#line 200 "Content\Casanova semantics\transform.mc"
            if (tmp_2.P1 == true)
            {
              var tmp_3 = tmp_1.P2 as _Then;
#line 200 "Content\Casanova semantics\transform.mc"
              if (tmp_3 != null)
              {
                var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else;
#line 200 "Content\Casanova semantics\transform.mc"
                if (tmp_4 != null)
                {
                  var e = tmp_1.P5;
#line 200 "Content\Casanova semantics\transform.mc"
                  if (M is Locals && lv is Locals && t is Expr)
                  {
#line 200 "Content\Casanova semantics\transform.mc"
                    var tmp_6 = eval.Create(dt, M as Locals, lv as Locals, t as Expr);
#line 200 "Content\Casanova semantics\transform.mc"
                    foreach (var tmp_5 in tmp_6.Run3_())
                    {
                      var res = tmp_5;
#line 200 "Content\Casanova semantics\transform.mc"
                      var result = res;
#line 200 "Content\Casanova semantics\transform.mc"
                      yield return result;
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 204 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _If;
#line 204 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var tmp_2 = tmp_1.P1 as _opDollarb;
#line 204 "Content\Casanova semantics\transform.mc"
          if (tmp_2 != null)
          {
#line 204 "Content\Casanova semantics\transform.mc"
            if (tmp_2.P1 == false)
            {
              var tmp_3 = tmp_1.P2 as _Then;
#line 204 "Content\Casanova semantics\transform.mc"
              if (tmp_3 != null)
              {
                var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else;
#line 204 "Content\Casanova semantics\transform.mc"
                if (tmp_4 != null)
                {
                  var e = tmp_1.P5;
#line 204 "Content\Casanova semantics\transform.mc"
                  if (M is Locals && lv is Locals && e is Expr)
                  {
#line 204 "Content\Casanova semantics\transform.mc"
                    var tmp_6 = eval.Create(dt, M as Locals, lv as Locals, e as Expr);
#line 204 "Content\Casanova semantics\transform.mc"
                    foreach (var tmp_5 in tmp_6.Run3_())
                    {
                      var res = tmp_5;
#line 204 "Content\Casanova semantics\transform.mc"
                      var result = res;
#line 204 "Content\Casanova semantics\transform.mc"
                      yield return result;
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 208 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var lv = tmp_0.P2; var M = tmp_0.P3; var tmp_1 = tmp_0.P4 as _opAddition;
#line 208 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var e1 = tmp_1.P1; var e2 = tmp_1.P2;
#line 208 "Content\Casanova semantics\transform.mc"
          if (M is Locals && lv is Locals && e1 is Expr)
          {
#line 208 "Content\Casanova semantics\transform.mc"
            var tmp_3 = eval.Create(dt, M as Locals, lv as Locals, e1 as Expr);
#line 208 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_())
            {
              var tmp_4 = tmp_2 as _opDollari;
#line 208 "Content\Casanova semantics\transform.mc"
              if (tmp_4 != null)
              {
                var res1 = tmp_4.P1;
#line 208 "Content\Casanova semantics\transform.mc"
                if (M is Locals && lv is Locals && e2 is Expr)
                {
#line 208 "Content\Casanova semantics\transform.mc"
                  var tmp_6 = eval.Create(dt, M as Locals, lv as Locals, e2 as Expr);
#line 208 "Content\Casanova semantics\transform.mc"
                  foreach (var tmp_5 in tmp_6.Run3_())
                  {
                    var tmp_7 = tmp_5 as _opDollari;
#line 208 "Content\Casanova semantics\transform.mc"
                    if (tmp_7 != null)
                    {
                      var res2 = tmp_7.P1; var res = (res1 + res2);
#line 208 "Content\Casanova semantics\transform.mc"
                      var result = _opDollari.Create(res);
#line 208 "Content\Casanova semantics\transform.mc"
                      yield return result;
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 214 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _opSubtraction;
#line 214 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var e1 = tmp_1.P1; var e2 = tmp_1.P2;
#line 214 "Content\Casanova semantics\transform.mc"
          if (M is Locals && lv is Locals && e1 is Expr)
          {
#line 214 "Content\Casanova semantics\transform.mc"
            var tmp_3 = eval.Create(dt, M as Locals, lv as Locals, e1 as Expr);
#line 214 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_())
            {
              var tmp_4 = tmp_2 as _opDollari;
#line 214 "Content\Casanova semantics\transform.mc"
              if (tmp_4 != null)
              {
                var res1 = tmp_4.P1;
#line 214 "Content\Casanova semantics\transform.mc"
                if (M is Locals && lv is Locals && e2 is Expr)
                {
#line 214 "Content\Casanova semantics\transform.mc"
                  var tmp_6 = eval.Create(dt, M as Locals, lv as Locals, e2 as Expr);
#line 214 "Content\Casanova semantics\transform.mc"
                  foreach (var tmp_5 in tmp_6.Run3_())
                  {
                    var tmp_7 = tmp_5 as _opDollari;
#line 214 "Content\Casanova semantics\transform.mc"
                    if (tmp_7 != null)
                    {
                      var res2 = tmp_7.P1; var res = (res1 - res2);
#line 214 "Content\Casanova semantics\transform.mc"
                      var result = _opDollari.Create(res);
#line 214 "Content\Casanova semantics\transform.mc"
                      yield return result;
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 220 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _opMultiplication;
#line 220 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var e1 = tmp_1.P1; var e2 = tmp_1.P2;
#line 220 "Content\Casanova semantics\transform.mc"
          if (M is Locals && lv is Locals && e1 is Expr)
          {
#line 220 "Content\Casanova semantics\transform.mc"
            var tmp_3 = eval.Create(dt, M as Locals, lv as Locals, e1 as Expr);
#line 220 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_())
            {
              var tmp_4 = tmp_2 as _opDollari;
#line 220 "Content\Casanova semantics\transform.mc"
              if (tmp_4 != null)
              {
                var res1 = tmp_4.P1;
#line 220 "Content\Casanova semantics\transform.mc"
                if (M is Locals && lv is Locals && e2 is Expr)
                {
#line 220 "Content\Casanova semantics\transform.mc"
                  var tmp_6 = eval.Create(dt, M as Locals, lv as Locals, e2 as Expr);
#line 220 "Content\Casanova semantics\transform.mc"
                  foreach (var tmp_5 in tmp_6.Run3_())
                  {
                    var tmp_7 = tmp_5 as _opDollari;
#line 220 "Content\Casanova semantics\transform.mc"
                    if (tmp_7 != null)
                    {
                      var res2 = tmp_7.P1; var res = (res1 * res2);
#line 220 "Content\Casanova semantics\transform.mc"
                      var result = _opDollari.Create(res);
#line 220 "Content\Casanova semantics\transform.mc"
                      yield return result;
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 226 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _opDivision;
#line 226 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var e1 = tmp_1.P1; var e2 = tmp_1.P2;
#line 226 "Content\Casanova semantics\transform.mc"
          if (M is Locals && lv is Locals && e1 is Expr)
          {
#line 226 "Content\Casanova semantics\transform.mc"
            var tmp_3 = eval.Create(dt, M as Locals, lv as Locals, e1 as Expr);
#line 226 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_())
            {
              var tmp_4 = tmp_2 as _opDollari;
#line 226 "Content\Casanova semantics\transform.mc"
              if (tmp_4 != null)
              {
                var res1 = tmp_4.P1;
#line 226 "Content\Casanova semantics\transform.mc"
                if (M is Locals && lv is Locals && e2 is Expr)
                {
#line 226 "Content\Casanova semantics\transform.mc"
                  var tmp_6 = eval.Create(dt, M as Locals, lv as Locals, e2 as Expr);
#line 226 "Content\Casanova semantics\transform.mc"
                  foreach (var tmp_5 in tmp_6.Run3_())
                  {
                    var tmp_7 = tmp_5 as _opDollari;
#line 226 "Content\Casanova semantics\transform.mc"
                    if (tmp_7 != null)
                    {
                      var res2 = tmp_7.P1; var res = (res1 / res2);
#line 226 "Content\Casanova semantics\transform.mc"
                      var result = _opDollari.Create(res);
#line 226 "Content\Casanova semantics\transform.mc"
                      yield return result;
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 232 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as wait;
#line 232 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var t = tmp_1.P1;
#line 232 "Content\Casanova semantics\transform.mc"
          if ((dt >= t).Equals(true))
          {
#line 232 "Content\Casanova semantics\transform.mc"
            var result = atomic.Create();
#line 232 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }


      {
#line 236 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as wait;
#line 236 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var t = tmp_1.P1;
#line 236 "Content\Casanova semantics\transform.mc"
          if ((dt < t).Equals(true))
          {
            var t_Prime = (t - dt);
#line 236 "Content\Casanova semantics\transform.mc"
            var result = waitResult.Create(t_Prime);
#line 236 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }


      {
#line 241 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _opDollari;
#line 241 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var val = tmp_1.P1;
#line 241 "Content\Casanova semantics\transform.mc"
          var result = _opDollari.Create(val);
#line 241 "Content\Casanova semantics\transform.mc"
          yield return result;
        }
      }


      {
#line 244 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _opDollar;
#line 244 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var id = tmp_1.P1;
#line 244 "Content\Casanova semantics\transform.mc"
          if (lv is Locals)
          {
#line 244 "Content\Casanova semantics\transform.mc"
            var tmp_3 = lookup.Create(lv as Locals, id);
#line 244 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_())
            {
              var val = tmp_2;
#line 244 "Content\Casanova semantics\transform.mc"
              var result = val;
#line 244 "Content\Casanova semantics\transform.mc"
              yield return result;
            }
          }
        }
      }


      {
#line 248 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as nil;
#line 248 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
#line 248 "Content\Casanova semantics\transform.mc"
          var result = nilResult.Create();
#line 248 "Content\Casanova semantics\transform.mc"
          yield return result;
        }
      }


      {
#line 251 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as yield;
#line 251 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var tmp_2 = tmp_1.P1 as _Semicolon;
#line 251 "Content\Casanova semantics\transform.mc"
          if (tmp_2 != null)
          {
            var e = tmp_2.P1; var exprs = tmp_2.P2;
#line 251 "Content\Casanova semantics\transform.mc"
            if (M is Locals && lv is Locals && e is Expr && exprs is ExprList)
            {
#line 251 "Content\Casanova semantics\transform.mc"
              var tmp_4 = evalMany.Create(dt, M as Locals, lv as Locals, _Semicolon.Create(e as Expr, exprs as ExprList));
#line 251 "Content\Casanova semantics\transform.mc"
              foreach (var tmp_3 in tmp_4.Run3_23_())
              {
                var vals = tmp_3;
#line 251 "Content\Casanova semantics\transform.mc"
                if (vals is ExprResultList)
                {
#line 251 "Content\Casanova semantics\transform.mc"
                  var result = yieldResult.Create(vals as ExprResultList);
#line 251 "Content\Casanova semantics\transform.mc"
                  yield return result;
                }
              }
            }
          }
        }
      }


      {
#line 265 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _Semicolon;
#line 265 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var a = tmp_1.P1; var b = tmp_1.P2;
#line 265 "Content\Casanova semantics\transform.mc"
          if (M is Locals && lv is Locals && a is Expr)
          {
#line 265 "Content\Casanova semantics\transform.mc"
            var tmp_3 = eval.Create(dt, M as Locals, lv as Locals, a as Expr);
#line 265 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_24_())
            {
              var a1 = tmp_2;
#line 265 "Content\Casanova semantics\transform.mc"
              if (M is Locals && a1 is ExprResult && b is Expr)
              {
#line 265 "Content\Casanova semantics\transform.mc"
                var tmp_5 = stepOrSuspend.Create(dt, M as Locals, a1 as ExprResult, b as Expr);
#line 265 "Content\Casanova semantics\transform.mc"
                foreach (var tmp_4 in tmp_5.Run3_24_())
                {
                  var res = tmp_4;
#line 265 "Content\Casanova semantics\transform.mc"
                  var result = res;
#line 265 "Content\Casanova semantics\transform.mc"
                  yield return result;
                }
              }
            }
          }
        }
      }

      foreach (var p in Run()) yield return p;
    }

    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " eval "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();
      res += P4.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as eval;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class evalMany : Expr
  {
    public float P1;
    public Locals P2;
    public Locals P3;
    public ExprList P4;

    public evalMany(float P1, Locals P2, Locals P3, ExprList P4) { this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; }
    public static evalMany Create(float P1, Locals P2, Locals P3, ExprList P4) { return new evalMany(P1, P2, P3, P4); }

    public IEnumerable<IRunnable> Run3_23_()
    {
      {
#line 255 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as _Semicolon;
#line 255 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var e = tmp_1.P1; var exprs = tmp_1.P2;
#line 255 "Content\Casanova semantics\transform.mc"
          if (M is Locals && lv is Locals && e is Expr)
          {
#line 255 "Content\Casanova semantics\transform.mc"
            var tmp_3 = eval.Create(dt, M as Locals, lv as Locals, e as Expr);
#line 255 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_23_())
            {
              var val = tmp_2;
#line 255 "Content\Casanova semantics\transform.mc"
              if (M is Locals && lv is Locals && exprs is ExprList)
              {
#line 255 "Content\Casanova semantics\transform.mc"
                var tmp_5 = evalMany.Create(dt, M as Locals, lv as Locals, exprs as ExprList);
#line 255 "Content\Casanova semantics\transform.mc"
                foreach (var tmp_4 in tmp_5.Run3_23_())
                {
                  var vals = tmp_4;
#line 255 "Content\Casanova semantics\transform.mc"
                  if (val is ExprResult && vals is ExprResultList)
                  {
                    var res = consResult.Create(val as ExprResult, vals as ExprResultList);
#line 255 "Content\Casanova semantics\transform.mc"
                    if (val is ExprResult && vals is ExprResultList)
                    {
#line 255 "Content\Casanova semantics\transform.mc"
                      var result = consResult.Create(val as ExprResult, vals as ExprResultList);
#line 255 "Content\Casanova semantics\transform.mc"
                      yield return result;
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 261 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var tmp_1 = tmp_0.P4 as nil;
#line 261 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
#line 261 "Content\Casanova semantics\transform.mc"
          var result = nilResult.Create();
#line 261 "Content\Casanova semantics\transform.mc"
          yield return result;
        }
      }

      foreach (var p in Run3_()) yield return p;
    }

    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " evalMany "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();
      res += P4.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as evalMany;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class evalRule : RuleEvaluation
  {
    public float P1;
    public Locals P2;
    public Locals P3;
    public Domain P4;
    public ExprList P5;
    public ExprList P6;

    public evalRule(float P1, Locals P2, Locals P3, Domain P4, ExprList P5, ExprList P6) { this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5; this.P6 = P6; }
    public static evalRule Create(float P1, Locals P2, Locals P3, Domain P4, ExprList P5, ExprList P6) { return new evalRule(P1, P2, P3, P4, P5, P6); }

    public IEnumerable<IRunnable> Run3_()
    {
      {
#line 157 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var domain = tmp_0.P4; var startingBlock = tmp_0.P5; var block = tmp_0.P6;
#line 157 "Content\Casanova semantics\transform.mc"
        if (M is Locals && lv is Locals && block is Expr)
        {
#line 157 "Content\Casanova semantics\transform.mc"
          var tmp_2 = eval.Create(dt, M as Locals, lv as Locals, block as Expr);
#line 157 "Content\Casanova semantics\transform.mc"
          foreach (var tmp_1 in tmp_2.Run3_())
          {
            var tmp_3 = tmp_1 as continueWith;
#line 157 "Content\Casanova semantics\transform.mc"
            if (tmp_3 != null)
            {
              var tmp_4 = tmp_3.P1 as _Semicolon;
#line 157 "Content\Casanova semantics\transform.mc"
              if (tmp_4 != null)
              {
                var tmp_5 = tmp_4.P1 as wait;
#line 157 "Content\Casanova semantics\transform.mc"
                if (tmp_5 != null)
                {
                  var t = tmp_5.P1; var b = tmp_4.P2;
#line 157 "Content\Casanova semantics\transform.mc"
                  if (M is Locals && lv is Locals && b is ExprList)
                  {
#line 157 "Content\Casanova semantics\transform.mc"
                    var result = ruleResult.Create(M as Locals, lv as Locals, _Semicolon.Create(wait.Create(t), b as ExprList));
#line 157 "Content\Casanova semantics\transform.mc"
                    yield return result;
                  }
                }
              }
            }
          }
        }
      }


      {
#line 161 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var domain = tmp_0.P4; var startingBlock = tmp_0.P5; var block = tmp_0.P6;
#line 161 "Content\Casanova semantics\transform.mc"
        if (M is Locals && lv is Locals && block is Expr)
        {
#line 161 "Content\Casanova semantics\transform.mc"
          var tmp_2 = eval.Create(dt, M as Locals, lv as Locals, block as Expr);
#line 161 "Content\Casanova semantics\transform.mc"
          foreach (var tmp_1 in tmp_2.Run3_6_())
          {
            var tmp_3 = tmp_1 as updateFieldsAndContinueWith;
#line 161 "Content\Casanova semantics\transform.mc"
            if (tmp_3 != null)
            {
              var vals = tmp_3.P1; var b = tmp_3.P2;
#line 161 "Content\Casanova semantics\transform.mc"
              if (M is Locals && domain is Domain && vals is ExprResultList)
              {
#line 161 "Content\Casanova semantics\transform.mc"
                var tmp_5 = updateFields.Create(M as Locals, domain as Domain, vals as ExprResultList);
#line 161 "Content\Casanova semantics\transform.mc"
                foreach (var tmp_4 in tmp_5.Run3_6_())
                {
                  var M_Prime = tmp_4;
#line 161 "Content\Casanova semantics\transform.mc"
                  if (M_Prime is Locals && lv is Locals && b is ExprList)
                  {
#line 161 "Content\Casanova semantics\transform.mc"
                    var result = ruleResult.Create(M_Prime as Locals, lv as Locals, b as ExprList);
#line 161 "Content\Casanova semantics\transform.mc"
                    yield return result;
                  }
                }
              }
            }
          }
        }
      }


      {
#line 175 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var domain = tmp_0.P4; var startingBlock = tmp_0.P5; var block = tmp_0.P6;
#line 175 "Content\Casanova semantics\transform.mc"
        if (M is Locals && lv is Locals && block is Expr)
        {
#line 175 "Content\Casanova semantics\transform.mc"
          var tmp_2 = eval.Create(dt, M as Locals, lv as Locals, block as Expr);
#line 175 "Content\Casanova semantics\transform.mc"
          foreach (var tmp_1 in tmp_2.Run3_())
          {
            var tmp_3 = tmp_1 as binding;
#line 175 "Content\Casanova semantics\transform.mc"
            if (tmp_3 != null)
            {
              var tmp_4 = tmp_3.P1 as _opDollar;
#line 175 "Content\Casanova semantics\transform.mc"
              if (tmp_4 != null)
              {
                var varName = tmp_4.P1; var val = tmp_3.P2; var b = tmp_3.P3;
#line 175 "Content\Casanova semantics\transform.mc"
                if (lv is Locals)
                {
#line 175 "Content\Casanova semantics\transform.mc"
                  var tmp_6 = add.Create(lv as Locals, varName, val);
#line 175 "Content\Casanova semantics\transform.mc"
                  foreach (var tmp_5 in tmp_6.Run3_())
                  {
                    var lv_Prime = tmp_5;
#line 175 "Content\Casanova semantics\transform.mc"
                    if (M is Locals && lv_Prime is Locals && domain is Domain && startingBlock is ExprList && b is ExprList)
                    {
#line 175 "Content\Casanova semantics\transform.mc"
                      var tmp_8 = evalRule.Create(dt, M as Locals, lv_Prime as Locals, domain as Domain, startingBlock as ExprList, b as ExprList);
#line 175 "Content\Casanova semantics\transform.mc"
                      foreach (var tmp_7 in tmp_8.Run3_())
                      {
                        var res = tmp_7;
#line 175 "Content\Casanova semantics\transform.mc"
                        var result = res;
#line 175 "Content\Casanova semantics\transform.mc"
                        yield return result;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 181 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var domain = tmp_0.P4; var startingBlock = tmp_0.P5; var block = tmp_0.P6;
#line 181 "Content\Casanova semantics\transform.mc"
        if (M is Locals && lv is Locals && block is Expr)
        {
#line 181 "Content\Casanova semantics\transform.mc"
          var tmp_2 = eval.Create(dt, M as Locals, lv as Locals, block as Expr);
#line 181 "Content\Casanova semantics\transform.mc"
          foreach (var tmp_1 in tmp_2.Run3_())
          {
            var tmp_3 = tmp_1 as reEvaluate;
#line 181 "Content\Casanova semantics\transform.mc"
            if (tmp_3 != null)
            {
              var b = tmp_3.P1;
#line 181 "Content\Casanova semantics\transform.mc"
              if (M is Locals && lv is Locals && domain is Domain && startingBlock is ExprList && b is ExprList)
              {
#line 181 "Content\Casanova semantics\transform.mc"
                var tmp_5 = evalRule.Create(dt, M as Locals, lv as Locals, domain as Domain, startingBlock as ExprList, b as ExprList);
#line 181 "Content\Casanova semantics\transform.mc"
                foreach (var tmp_4 in tmp_5.Run3_())
                {
                  var res = tmp_4;
#line 181 "Content\Casanova semantics\transform.mc"
                  var result = res;
#line 181 "Content\Casanova semantics\transform.mc"
                  yield return result;
                }
              }
            }
          }
        }
      }


      {
#line 186 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var lv = tmp_0.P3; var domain = tmp_0.P4; var startingBlock = tmp_0.P5; var tmp_1 = tmp_0.P6 as nil;
#line 186 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
#line 186 "Content\Casanova semantics\transform.mc"
          if (M is Locals && lv is Locals && startingBlock is ExprList)
          {
#line 186 "Content\Casanova semantics\transform.mc"
            var result = ruleResult.Create(M as Locals, lv as Locals, startingBlock as ExprList);
#line 186 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }

      foreach (var p in Run()) yield return p;
    }

    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " evalRule "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();
      res += P4.ToString();
      res += P5.ToString();
      res += P6.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as evalRule;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4) && this.P5.Equals(tmp.P5) && this.P6.Equals(tmp.P6);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _If : Expr
  {
    public BoolExpr P1;
    public Then P2;
    public ExprList P3;
    public Else P4;
    public ExprList P5;

    public _If(BoolExpr P1, Then P2, ExprList P3, Else P4, ExprList P5) { this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5; }
    public static _If Create(BoolExpr P1, Then P2, ExprList P3, Else P4, ExprList P5) { return new _If(P1, P2, P3, P4, P5); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " if "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();
      res += P4.ToString();
      res += P5.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _If;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4) && this.P5.Equals(tmp.P5);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class let : Expr
  {
    public Id P1;
    public Expr P2;

    public let(Id P1, Expr P2) { this.P1 = P1; this.P2 = P2; }
    public static let Create(Id P1, Expr P2) { return new let(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " let "; res += P1.ToString();
      res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as let;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class letResult : ExprResult
  {
    public Id P1;
    public ExprResult P2;

    public letResult(Id P1, ExprResult P2) { this.P1 = P1; this.P2 = P2; }
    public static letResult Create(Id P1, ExprResult P2) { return new letResult(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " letResult "; res += P1.ToString();
      res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as letResult;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class lookup : MemoryOp
  {
    public Locals P1;
    public string P2;

    public lookup(Locals P1, string P2) { this.P1 = P1; this.P2 = P2; }
    public static lookup Create(Locals P1, string P2) { return new lookup(P1, P2); }

    public IEnumerable<IRunnable> Run()
    {
      {
#line 87 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm;
#line 87 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var M = tmp_1.P1; var k = tmp_0.P2; var v = (M.GetKey(k));
#line 87 "Content\Casanova semantics\transform.mc"
          var result = v;
#line 87 "Content\Casanova semantics\transform.mc"
          yield return result;
        }
      }

    }

    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " lookup "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach (var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}"; } else { res += P2.ToString(); }

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as lookup;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class loopRules : RuleManager
  {
    public float P1;
    public Locals P2;
    public RuleList P3;
    public RuleList P4;
    public int P5;

    public loopRules(float P1, Locals P2, RuleList P3, RuleList P4, int P5) { this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5; }
    public static loopRules Create(float P1, Locals P2, RuleList P3, RuleList P4, int P5) { return new loopRules(P1, P2, P3, P4, P5); }

    public IEnumerable<IRunnable> Run3_()
    {
      {
#line 134 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var fields = tmp_0.P2; var startingRules = tmp_0.P3; var rs = tmp_0.P4; var i = tmp_0.P5;
#line 134 "Content\Casanova semantics\transform.mc"
        if ((i > 0).Equals(true))
        {
          var outputString = ("\n----------------\n" + (fields.ToString()) + "\n\n" + (rs.ToString()) + "\n----------------\n"); var outputUpdate = (EntryPoint.Print(outputString)); var sleeping = (EntryPoint.Sleep(dt));
#line 134 "Content\Casanova semantics\transform.mc"
          if (fields is Locals && startingRules is RuleList && rs is RuleList)
          {
#line 134 "Content\Casanova semantics\transform.mc"
            var tmp_2 = updateRules.Create(dt, fields as Locals, startingRules as RuleList, rs as RuleList);
#line 134 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_1 in tmp_2.Run3_())
            {
              var tmp_3 = tmp_1 as updateResult;
#line 134 "Content\Casanova semantics\transform.mc"
              if (tmp_3 != null)
              {
                var updatedFields = tmp_3.P1; var updatedRules = tmp_3.P2; var j = (i - 1);
#line 134 "Content\Casanova semantics\transform.mc"
                if (updatedFields is Locals && startingRules is RuleList && updatedRules is RuleList)
                {
#line 134 "Content\Casanova semantics\transform.mc"
                  var tmp_5 = loopRules.Create(dt, updatedFields as Locals, startingRules as RuleList, updatedRules as RuleList, j);
#line 134 "Content\Casanova semantics\transform.mc"
                  foreach (var tmp_4 in tmp_5.Run3_())
                  {
                    var tmp_6 = tmp_4 as updateResult;
#line 134 "Content\Casanova semantics\transform.mc"
                    if (tmp_6 != null)
                    {
                      var fs_Prime = tmp_6.P1; var rs_Prime = tmp_6.P2;
#line 134 "Content\Casanova semantics\transform.mc"
                      if (fs_Prime is Locals && rs_Prime is RuleList)
                      {
#line 134 "Content\Casanova semantics\transform.mc"
                        var result = updateResult.Create(fs_Prime as Locals, rs_Prime as RuleList);
#line 134 "Content\Casanova semantics\transform.mc"
                        yield return result;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 144 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var fields = tmp_0.P2; var startingRules = tmp_0.P3; var rs = tmp_0.P4;
#line 144 "Content\Casanova semantics\transform.mc"
        if (tmp_0.P5 == 0)
        {
#line 144 "Content\Casanova semantics\transform.mc"
          if (fields is Locals && rs is RuleList)
          {
#line 144 "Content\Casanova semantics\transform.mc"
            var result = updateResult.Create(fields as Locals, rs as RuleList);
#line 144 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }

      foreach (var p in Run()) yield return p;
    }

    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " loopRules "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();
      res += P4.ToString();
      res += P5.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as loopRules;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4) && this.P5.Equals(tmp.P5);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class nil : ExprList
  {

    public nil() { }
    public static nil Create() { return new nil(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "nil";
    }

    public override bool Equals(object other)
    {
      return other is nil;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class nilDomain : Domain
  {

    public nilDomain() { }
    public static nilDomain Create() { return new nilDomain(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "nilDomain";
    }

    public override bool Equals(object other)
    {
      return other is nilDomain;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class nilResult : ExprResultList
  {

    public nilResult() { }
    public static nilResult Create() { return new nilResult(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "nilResult";
    }

    public override bool Equals(object other)
    {
      return other is nilResult;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class nilRule : RuleList
  {

    public nilRule() { }
    public static nilRule Create() { return new nilRule(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "nilRule";
    }

    public override bool Equals(object other)
    {
      return other is nilRule;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class reEvaluate : ExprResult
  {
    public ExprList P1;

    public reEvaluate(ExprList P1) { this.P1 = P1; }
    public static reEvaluate Create(ExprList P1) { return new reEvaluate(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " reEvaluate "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as reEvaluate;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class rule : Rule
  {
    public Domain P1;
    public ExprList P2;
    public Locals P3;

    public rule(Domain P1, ExprList P2, Locals P3) { this.P1 = P1; this.P2 = P2; this.P3 = P3; }
    public static rule Create(Domain P1, ExprList P2, Locals P3) { return new rule(P1, P2, P3); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " rule "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as rule;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class ruleResult : RuleResult
  {
    public Locals P1;
    public Locals P2;
    public ExprList P3;

    public ruleResult(Locals P1, Locals P2, ExprList P3) { this.P1 = P1; this.P2 = P2; this.P3 = P3; }
    public static ruleResult Create(Locals P1, Locals P2, ExprList P3) { return new ruleResult(P1, P2, P3); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " ruleResult "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as ruleResult;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class runTest1 : Test
  {

    public runTest1() { }
    public static runTest1 Create() { return new runTest1(); }

    public IEnumerable<IRunnable> Run()
    {
      {
#line 103 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this as runTest1; var dt = 1.000000f; var iterations = 10; var Me = _opDollarm.Create(System.Collections.Immutable.ImmutableDictionary<string, ExprResult>.Empty);
#line 103 "Content\Casanova semantics\transform.mc"
        if (Me is Locals)
        {
#line 103 "Content\Casanova semantics\transform.mc"
          var tmp_2 = add.Create(Me as Locals, "F1", _opDollari.Create(100));
#line 103 "Content\Casanova semantics\transform.mc"
          foreach (var tmp_1 in tmp_2.Run3_())
          {
            var Ml = tmp_1;
#line 103 "Content\Casanova semantics\transform.mc"
            if (Ml is Locals)
            {
#line 103 "Content\Casanova semantics\transform.mc"
              var tmp_4 = add.Create(Ml as Locals, "F2", _opDollari.Create(100));
#line 103 "Content\Casanova semantics\transform.mc"
              foreach (var tmp_3 in tmp_4.Run3_())
              {
                var M = tmp_3; var L1 = _opDollarm.Create(System.Collections.Immutable.ImmutableDictionary<string, ExprResult>.Empty); var L2 = _opDollarm.Create(System.Collections.Immutable.ImmutableDictionary<string, ExprResult>.Empty); var dom1 = consDomain.Create("F1", nilDomain.Create()); var dom2 = consDomain.Create("F2", nilDomain.Create()); var f1 = _Semicolon.Create(_opDollari.Create(90), _Semicolon.Create(_opDollari.Create(50), nil.Create())); var l1 = let.Create(_opDollar.Create("x"), _opDollari.Create(100)); var w1 = wait.Create(2.000000f); var w2 = wait.Create(2.000000f); var y1 = yield.Create(_Semicolon.Create(_opAddition.Create(_opDollari.Create(10), _opDollari.Create(30)), nil.Create())); var y2 = yield.Create(_Semicolon.Create(_opDollari.Create(20), nil.Create())); var y3 = yield.Create(_Semicolon.Create(_opDollar.Create("x"), nil.Create()));
#line 103 "Content\Casanova semantics\transform.mc"
                if (l1 is Expr && w1 is Expr && y1 is Expr && w1 is Expr && y3 is Expr)
                {
                  var b1 = _Semicolon.Create(l1 as Expr, _Semicolon.Create(w1 as Expr, _Semicolon.Create(y1 as Expr, _Semicolon.Create(w1 as Expr, _Semicolon.Create(y3 as Expr, nil.Create())))));
#line 103 "Content\Casanova semantics\transform.mc"
                  if (w2 is Expr && y2 is Expr)
                  {
                    var b2 = _Semicolon.Create(w2 as Expr, _Semicolon.Create(y2 as Expr, nil.Create()));
#line 103 "Content\Casanova semantics\transform.mc"
                    if (dom1 is Domain && b1 is ExprList && L1 is Locals)
                    {
                      var r1 = rule.Create(dom1 as Domain, b1 as ExprList, L1 as Locals);
#line 103 "Content\Casanova semantics\transform.mc"
                      if (dom2 is Domain && b2 is ExprList && L2 is Locals)
                      {
                        var r2 = rule.Create(dom2 as Domain, b2 as ExprList, L2 as Locals);
#line 103 "Content\Casanova semantics\transform.mc"
                        if (r1 is Rule)
                        {
                          var rs = consRule.Create(r1 as Rule, nilRule.Create());
#line 103 "Content\Casanova semantics\transform.mc"
                          if (M is Locals && rs is RuleList)
                          {
                            var e = entity.Create("E", M as Locals, rs as RuleList);
#line 103 "Content\Casanova semantics\transform.mc"
                            if (e is Entity)
                            {
#line 103 "Content\Casanova semantics\transform.mc"
                              var tmp_6 = updateEntity.Create(dt, e as Entity, iterations);
#line 103 "Content\Casanova semantics\transform.mc"
                              foreach (var tmp_5 in tmp_6.Run3_())
                              {
                                var res = tmp_5; var debug = (EntryPoint.Print("Done!"));
#line 103 "Content\Casanova semantics\transform.mc"
                                var result = res;
#line 103 "Content\Casanova semantics\transform.mc"
                                yield return result;
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

    }

    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "runTest1";
    }

    public override bool Equals(object other)
    {
      return other is runTest1;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class setDt : ExprResult
  {
    public float P1;

    public setDt(float P1) { this.P1 = P1; }
    public static setDt Create(float P1) { return new setDt(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " setDt "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as setDt;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class stepOrSuspend : Expr
  {
    public float P1;
    public Locals P2;
    public ExprResult P3;
    public Expr P4;

    public stepOrSuspend(float P1, Locals P2, ExprResult P3, Expr P4) { this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; }
    public static stepOrSuspend Create(float P1, Locals P2, ExprResult P3, Expr P4) { return new stepOrSuspend(P1, P2, P3, P4); }

    public IEnumerable<IRunnable> Run3_24_()
    {
      {
#line 270 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as atomic;
#line 270 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var b = tmp_0.P4;
#line 270 "Content\Casanova semantics\transform.mc"
          if (b is ExprList)
          {
            var res = reEvaluate.Create(b as ExprList);
#line 270 "Content\Casanova semantics\transform.mc"
            var result = res;
#line 270 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }


      {
#line 274 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as letResult;
#line 274 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var id = tmp_1.P1; var exp = tmp_1.P2; var b = tmp_0.P4;
#line 274 "Content\Casanova semantics\transform.mc"
          if (id is Id && exp is ExprResult && b is ExprList)
          {
            var res = binding.Create(id as Id, exp as ExprResult, b as ExprList);
#line 274 "Content\Casanova semantics\transform.mc"
            var result = res;
#line 274 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }


      {
#line 278 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as nilResult;
#line 278 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var b = tmp_0.P4;
#line 278 "Content\Casanova semantics\transform.mc"
          if (b is ExprList)
          {
            var res = reEvaluate.Create(b as ExprList);
#line 278 "Content\Casanova semantics\transform.mc"
            var result = res;
#line 278 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }


      {
#line 282 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as waitResult;
#line 282 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var t = tmp_1.P1; var b = tmp_0.P4;
#line 282 "Content\Casanova semantics\transform.mc"
          if (b is ExprList)
          {
            var res = continueWith.Create(_Semicolon.Create(wait.Create(t), b as ExprList));
#line 282 "Content\Casanova semantics\transform.mc"
            var result = res;
#line 282 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }


      {
#line 286 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as yieldResult;
#line 286 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var vals = tmp_1.P1; var b = tmp_0.P4;
#line 286 "Content\Casanova semantics\transform.mc"
          if (vals is ExprResultList && b is ExprList)
          {
            var res = updateFieldsAndContinueWith.Create(vals as ExprResultList, b as ExprList);
#line 286 "Content\Casanova semantics\transform.mc"
            var result = res;
#line 286 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }

      foreach (var p in Run3_()) yield return p;
    }

    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " stepOrSuspend "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();
      res += P4.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as stepOrSuspend;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class suspend : ExprResult
  {

    public suspend() { }
    public static suspend Create() { return new suspend(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "suspend";
    }

    public override bool Equals(object other)
    {
      return other is suspend;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class suspendResult : ExprResult
  {

    public suspendResult() { }
    public static suspendResult Create() { return new suspendResult(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "suspendResult";
    }

    public override bool Equals(object other)
    {
      return other is suspendResult;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _Then : Then
  {

    public _Then() { }
    public static _Then Create() { return new _Then(); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      return "then";
    }

    public override bool Equals(object other)
    {
      return other is _Then;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class updateEntity : EntityManager
  {
    public float P1;
    public Entity P2;
    public int P3;

    public updateEntity(float P1, Entity P2, int P3) { this.P1 = P1; this.P2 = P2; this.P3 = P3; }
    public static updateEntity Create(float P1, Entity P2, int P3) { return new updateEntity(P1, P2, P3); }

    public IEnumerable<IRunnable> Run3_()
    {
      {
#line 130 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var tmp_1 = tmp_0.P2 as entity;
#line 130 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var name = tmp_1.P1; var fields = tmp_1.P2; var rs = tmp_1.P3; var updates = tmp_0.P3;
#line 130 "Content\Casanova semantics\transform.mc"
          if (fields is Locals && rs is RuleList && rs is RuleList)
          {
#line 130 "Content\Casanova semantics\transform.mc"
            var tmp_3 = loopRules.Create(dt, fields as Locals, rs as RuleList, rs as RuleList, updates);
#line 130 "Content\Casanova semantics\transform.mc"
            foreach (var tmp_2 in tmp_3.Run3_())
            {
              var tmp_4 = tmp_2 as updateResult;
#line 130 "Content\Casanova semantics\transform.mc"
              if (tmp_4 != null)
              {
                var fields_Prime = tmp_4.P1; var rs_Prime = tmp_4.P2;
#line 130 "Content\Casanova semantics\transform.mc"
                if (fields_Prime is Locals && rs_Prime is RuleList)
                {
#line 130 "Content\Casanova semantics\transform.mc"
                  var result = entity.Create(name, fields_Prime as Locals, rs_Prime as RuleList);
#line 130 "Content\Casanova semantics\transform.mc"
                  yield return result;
                }
              }
            }
          }
        }
      }

      foreach (var p in Run()) yield return p;
    }

    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " updateEntity "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as updateEntity;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class updateFields : MemoryOp
  {
    public Locals P1;
    public Domain P2;
    public ExprResultList P3;

    public updateFields(Locals P1, Domain P2, ExprResultList P3) { this.P1 = P1; this.P2 = P2; this.P3 = P3; }
    public static updateFields Create(Locals P1, Domain P2, ExprResultList P3) { return new updateFields(P1, P2, P3); }

    public IEnumerable<IRunnable> Run3_6_()
    {
      {
#line 166 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as consDomain;
#line 166 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var field = tmp_1.P1; var fields = tmp_1.P2; var tmp_2 = tmp_0.P3 as consResult;
#line 166 "Content\Casanova semantics\transform.mc"
          if (tmp_2 != null)
          {
            var v = tmp_2.P1; var vals = tmp_2.P2;
#line 166 "Content\Casanova semantics\transform.mc"
            if (M is Locals)
            {
#line 166 "Content\Casanova semantics\transform.mc"
              var tmp_4 = add.Create(M as Locals, field, v);
#line 166 "Content\Casanova semantics\transform.mc"
              foreach (var tmp_3 in tmp_4.Run3_6_())
              {
                var M_Prime = tmp_3;
#line 166 "Content\Casanova semantics\transform.mc"
                if (M_Prime is Locals && fields is Domain && vals is ExprResultList)
                {
#line 166 "Content\Casanova semantics\transform.mc"
                  var tmp_6 = updateFields.Create(M_Prime as Locals, fields as Domain, vals as ExprResultList);
#line 166 "Content\Casanova semantics\transform.mc"
                  foreach (var tmp_5 in tmp_6.Run3_6_())
                  {
                    var M_Prime_Prime = tmp_5;
#line 166 "Content\Casanova semantics\transform.mc"
                    var result = M_Prime_Prime;
#line 166 "Content\Casanova semantics\transform.mc"
                    yield return result;
                  }
                }
              }
            }
          }
        }
      }


      {
#line 171 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as nilDomain;
#line 171 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var tmp_2 = tmp_0.P3 as nilResult;
#line 171 "Content\Casanova semantics\transform.mc"
          if (tmp_2 != null)
          {
#line 171 "Content\Casanova semantics\transform.mc"
            var result = M;
#line 171 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }

      foreach (var p in Run3_()) yield return p;
    }

    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " updateFields "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as updateFields;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class updateFieldsAndContinueWith : ExprResult
  {
    public ExprResultList P1;
    public ExprList P2;

    public updateFieldsAndContinueWith(ExprResultList P1, ExprList P2) { this.P1 = P1; this.P2 = P2; }
    public static updateFieldsAndContinueWith Create(ExprResultList P1, ExprList P2) { return new updateFieldsAndContinueWith(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " updateFieldsAndContinueWith "; res += P1.ToString();
      res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as updateFieldsAndContinueWith;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class updateResult : UpdateResult
  {
    public Locals P1;
    public RuleList P2;

    public updateResult(Locals P1, RuleList P2) { this.P1 = P1; this.P2 = P2; }
    public static updateResult Create(Locals P1, RuleList P2) { return new updateResult(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " updateResult "; res += P1.ToString();
      res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as updateResult;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class updateRules : RuleManager
  {
    public float P1;
    public Locals P2;
    public RuleList P3;
    public RuleList P4;

    public updateRules(float P1, Locals P2, RuleList P3, RuleList P4) { this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; }
    public static updateRules Create(float P1, Locals P2, RuleList P3, RuleList P4) { return new updateRules(P1, P2, P3, P4); }

    public IEnumerable<IRunnable> Run3_()
    {
      {
#line 148 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as consRule;
#line 148 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
          var tmp_2 = tmp_1.P1 as rule;
#line 148 "Content\Casanova semantics\transform.mc"
          if (tmp_2 != null)
          {
            var sd = tmp_2.P1; var sb = tmp_2.P2; var sl = tmp_2.P3; var startingRules = tmp_1.P2; var tmp_3 = tmp_0.P4 as consRule;
#line 148 "Content\Casanova semantics\transform.mc"
            if (tmp_3 != null)
            {
              var tmp_4 = tmp_3.P1 as rule;
#line 148 "Content\Casanova semantics\transform.mc"
              if (tmp_4 != null)
              {
                var domain = tmp_4.P1; var block = tmp_4.P2; var localVariables = tmp_4.P3; var rs = tmp_3.P2;
#line 148 "Content\Casanova semantics\transform.mc"
                if (M is Locals && localVariables is Locals && domain is Domain && sb is ExprList && block is ExprList)
                {
#line 148 "Content\Casanova semantics\transform.mc"
                  var tmp_6 = evalRule.Create(dt, M as Locals, localVariables as Locals, domain as Domain, sb as ExprList, block as ExprList);
#line 148 "Content\Casanova semantics\transform.mc"
                  foreach (var tmp_5 in tmp_6.Run3_())
                  {
                    var tmp_7 = tmp_5 as ruleResult;
#line 148 "Content\Casanova semantics\transform.mc"
                    if (tmp_7 != null)
                    {
                      var M_Prime = tmp_7.P1; var localVariables_Prime = tmp_7.P2; var continuation = tmp_7.P3;
#line 148 "Content\Casanova semantics\transform.mc"
                      if (domain is Domain && continuation is ExprList && localVariables_Prime is Locals)
                      {
                        var updatedRule = rule.Create(domain as Domain, continuation as ExprList, localVariables_Prime as Locals);
#line 148 "Content\Casanova semantics\transform.mc"
                        if (M_Prime is Locals && startingRules is RuleList && rs is RuleList)
                        {
#line 148 "Content\Casanova semantics\transform.mc"
                          var tmp_9 = updateRules.Create(dt, M_Prime as Locals, startingRules as RuleList, rs as RuleList);
#line 148 "Content\Casanova semantics\transform.mc"
                          foreach (var tmp_8 in tmp_9.Run3_())
                          {
                            var tmp_10 = tmp_8 as updateResult;
#line 148 "Content\Casanova semantics\transform.mc"
                            if (tmp_10 != null)
                            {
                              var M_Prime_Prime = tmp_10.P1; var rs_Prime = tmp_10.P2;
#line 148 "Content\Casanova semantics\transform.mc"
                              if (M_Prime_Prime is Locals && updatedRule is Rule && rs_Prime is RuleList)
                              {
#line 148 "Content\Casanova semantics\transform.mc"
                                var result = updateResult.Create(M_Prime_Prime as Locals, consRule.Create(updatedRule as Rule, rs_Prime as RuleList));
#line 148 "Content\Casanova semantics\transform.mc"
                                yield return result;
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }


      {
#line 154 "Content\Casanova semantics\transform.mc"
        var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var rs = tmp_0.P3; var tmp_1 = tmp_0.P4 as nilRule;
#line 154 "Content\Casanova semantics\transform.mc"
        if (tmp_1 != null)
        {
#line 154 "Content\Casanova semantics\transform.mc"
          if (M is Locals)
          {
#line 154 "Content\Casanova semantics\transform.mc"
            var result = updateResult.Create(M as Locals, nilRule.Create());
#line 154 "Content\Casanova semantics\transform.mc"
            yield return result;
          }
        }
      }

      foreach (var p in Run()) yield return p;
    }

    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " updateRules "; res += P1.ToString();
      res += P2.ToString();
      res += P3.ToString();
      res += P4.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as updateRules;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class wait : Expr
  {
    public float P1;

    public wait(float P1) { this.P1 = P1; }
    public static wait Create(float P1) { return new wait(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " wait "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as wait;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class waitResult : ExprResult
  {
    public float P1;

    public waitResult(float P1) { this.P1 = P1; }
    public static waitResult Create(float P1) { return new waitResult(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " waitResult "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as waitResult;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class yield : Expr
  {
    public ExprList P1;

    public yield(ExprList P1) { this.P1 = P1; }
    public static yield Create(ExprList P1) { return new yield(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " yield "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as yield;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class yieldResult : ExprResult
  {
    public ExprResultList P1;

    public yieldResult(ExprResultList P1) { this.P1 = P1; }
    public static yieldResult Create(ExprResultList P1) { return new yieldResult(P1); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";

      res += " yieldResult "; res += P1.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as yieldResult;
      if (tmp != null) return this.P1.Equals(tmp.P1);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }

  public class _opOr : BoolExpr
  {
    public BoolExpr P1;
    public BoolExpr P2;

    public _opOr(BoolExpr P1, BoolExpr P2) { this.P1 = P1; this.P2 = P2; }
    public static _opOr Create(BoolExpr P1, BoolExpr P2) { return new _opOr(P1, P2); }


    public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0, 0)) yield return null; }
    public IEnumerable<IRunnable> Run3_() { foreach (var p in Run()) yield return p; }
    public IEnumerable<IRunnable> Run3_6_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_23_() { foreach (var p in Run3_()) yield return p; }
    public IEnumerable<IRunnable> Run3_24_() { foreach (var p in Run3_()) yield return p; }

    public override string ToString()
    {
      var res = "(";
      res += P1.ToString();

      res += " || "; res += P2.ToString();

      res += ")";
      return res;
    }

    public override bool Equals(object other)
    {
      var tmp = other as _opOr;
      if (tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2);
      else return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

  }




  public class EntryPoint
  {
    static public int Print(object s) { System.Console.WriteLine(s.ToString()); return 0; }
    static public int Sleep(float s) { int t = (int)(s * 1000.0f); System.Threading.Thread.Sleep(t); return 0; }
    static public IEnumerable<IRunnable> Run(bool printInput)
    {
#line 1 "input"
      System.Console.WriteLine("Done!");
      var p = runTest1.Create();
      if (printInput) System.Console.WriteLine(p.ToString());
      foreach (var x in p.Run())
        yield return x;
    }

    public static void Main(string[] args)
    {
      for (int i = 0; i < 10; i++)
      {
        Run(false);
      }
    }
  }

}
