namespace primitives {
	struct IntAdd {
		public static System.Int32 static_eval(System.Int32 l, System.Int32 r) {
			return l + r;
		}
		public System.Int32 l;
		public System.Int32 r;
		public System.Int32 eval() { return static_eval(l, r); }
	}

	struct IntSub {
		public static System.Int32 static_eval(System.Int32 l, System.Int32 r) {
			return l - r;
		}
		public System.Int32 l;
		public System.Int32 r;
		public System.Int32 eval() { return static_eval(l, r); }
	}

	struct IntMul {
		public static System.Int32 static_eval(System.Int32 l, System.Int32 r) {
			return l * r;
		}
		public System.Int32 l;
		public System.Int32 r;
		public System.Int32 eval() { return static_eval(l, r); }
	}

	struct IntDiv {
		public static System.Int32 static_eval(System.Int32 l, System.Int32 r) {
			return l / r;
		}
		public System.Int32 l;
		public System.Int32 r;
		public System.Int32 eval() { return static_eval(l, r); }
	}
}