namespace codegen_test {
  struct _Star_Double_Double {
    public _Star_Double_Double(double a, double b) { this.a = a; this.b = b; }
    public double a;
    public double b;
  }
  struct solve_quadratic {
    public static System.Collections.Generic.List<_Star_Double_Double> static_call(float a, float b, float c) {
      var _ret = new System.Collections.Generic.List<_Star_Double_Double>();
      {
        var tmp_0 = a * c;
        var tmp_1 = 4 * tmp_0;
        var tmp_2 = b * b;
        var d = tmp_2 - tmp_1;
        var tmp_3 = d > 0;
        if (tmp_3) {
          var tmp_4 = 0 - b;
          var tmp_5 = System.Math.Sqrt(d);
          var x = tmp_4 - tmp_5;
          var tmp_6 = b * x;
          var tmp_7 = x * x;
          var tmp_8 = a * tmp_7;
          var tmp_9 = tmp_6 + c;
          var y = tmp_8 + tmp_9;
          var tmp_10 = new _Star_Double_Double(x,y);
          _ret.Add(tmp_10);
        }
      }
      {
        var tmp_0 = a * c;
        var tmp_1 = 4 * tmp_0;
        var tmp_2 = b * b;
        var d = tmp_2 - tmp_1;
        var tmp_3 = d > 0;
        if (tmp_3) {
          var tmp_4 = 0 - b;
          var tmp_5 = System.Math.Sqrt(d);
          var x = tmp_4 + tmp_5;
          var tmp_6 = b * x;
          var tmp_7 = x * x;
          var tmp_8 = a * tmp_7;
          var tmp_9 = tmp_6 + c;
          var y = tmp_8 + tmp_9;
          var tmp_10 = new _Star_Double_Double(x, y);
          _ret.Add(tmp_10);
        }
      }
      {
        var tmp_0 = a * c;
        var tmp_1 = 4 * tmp_0;
        var tmp_2 = b * b;
        var d = tmp_2 - tmp_1;
        var tmp_3 = d == 0;
        if (tmp_3) {
          var x = 0 - b;
          var tmp_4 = b * x;
          var tmp_5 = x * x;
          var tmp_6 = a * tmp_5;
          var tmp_7 = tmp_4 + c;
          var y = tmp_6 + tmp_7;
          var tmp_8 = new _Star_Double_Double(x,y);
          _ret.Add(tmp_8);
        }
      }
      return _ret;
    }
    public System.Collections.Generic.List<_Star_Double_Double> dynamic_call() {
      return static_call(a, b, c);
    }
    public float a;
    public float b;
    public float c;
  }
  struct run {
    public static System.Collections.Generic.List<_Star_Double_Double> static_call() {
      var _ret = new System.Collections.Generic.List<_Star_Double_Double>();
      {
        var a = (float)  2.0;
        var b = (float) -4.0;
        var c = (float) -5.0;
        var tmp_0 = solve_quadratic.static_call(a, b, c);
        foreach (var tmp_1 in tmp_0) {
          _ret.Add(tmp_1);
        }
      }
      return _ret;
    }
    public System.Collections.Generic.List<_Star_Double_Double> dynamic_call() {
      return static_call();
    }
  }
}