struct _DBUG {
  public static System.Windows.Forms.TreeView program_tree = new System.Windows.Forms.TreeView();
  public static bool[][][][] breakpoints;
  public static void breakpoint(string filename, int line, System.Collections.Generic.Dictionary<string, object> stab) {
    // table
    var table = new System.Windows.Forms.ListView();
    table.GridLines = true;
    table.Dock = System.Windows.Forms.DockStyle.Fill;
    table.View = System.Windows.Forms.View.Details;
    table.Columns.Add("name", -2, System.Windows.Forms.HorizontalAlignment.Left);
    table.Columns.Add("value", -2, System.Windows.Forms.HorizontalAlignment.Left);
    foreach (var x in stab) {
      table.Items.Add(new System.Windows.Forms.ListViewItem(new string[] { x.Key, System.String.Format("{0}", x.Value) }));
    }

    var tablepanel = new System.Windows.Forms.Panel();
    tablepanel.Dock = System.Windows.Forms.DockStyle.Fill;
    tablepanel.Controls.Add(table);

    // tree
    program_tree.CheckBoxes = true;
    program_tree.Dock = System.Windows.Forms.DockStyle.Fill;
    program_tree.ExpandAll();
    for (int filenr = 0; filenr<breakpoints.Length; filenr++) {
      for (int funcnr = 0; funcnr < breakpoints[filenr].Length; funcnr++) {
        for (int rulenr = 0; rulenr < breakpoints[filenr][funcnr].Length; rulenr++) {
          for (int premnr = 0; premnr < breakpoints[filenr][funcnr][rulenr].Length; premnr++) {
            program_tree.Nodes[filenr].Nodes[funcnr].Nodes[rulenr].Nodes[premnr].Checked = breakpoints[filenr][funcnr][rulenr][premnr];
          }
        }
      }
    }

    var treepanel = new System.Windows.Forms.Panel();
    treepanel.Dock = System.Windows.Forms.DockStyle.Fill;
    treepanel.Controls.Add(program_tree);

    // split
    var split = new System.Windows.Forms.SplitContainer();
    split.SplitterDistance = 100;
    split.Dock = System.Windows.Forms.DockStyle.Fill;
    split.Panel1.Controls.Add(treepanel);
    split.Panel2.Controls.Add(tablepanel);

    var splitform = new System.Windows.Forms.Form();
    splitform.Controls.Add(split);
    splitform.MinimumSize = new System.Drawing.Size(100, 100);
    splitform.Text = filename + ":" + line;

    var buttons = new System.Windows.Forms.Button[4];
    buttons[0] = new System.Windows.Forms.Button();
    buttons[0].Text = "continue";
    buttons[0].DialogResult = System.Windows.Forms.DialogResult.Ignore;
    buttons[1] = new System.Windows.Forms.Button();
    buttons[1].Text = "abort";
    buttons[1].DialogResult = System.Windows.Forms.DialogResult.Abort;
    /*buttons[2] = new System.Windows.Forms.Button();
    buttons[2].Text = "step in";
    buttons[2].DialogResult = System.Windows.Forms.DialogResult.OK;
    buttons[3] = new System.Windows.Forms.Button();
    buttons[3].Text = "step over";
    buttons[3].DialogResult = System.Windows.Forms.DialogResult.Yes;*/

    var collection = new System.Windows.Forms.FlowLayoutPanel();
    collection.Dock = System.Windows.Forms.DockStyle.Bottom;
    collection.Controls.AddRange(buttons);
    collection.AutoSize = true;
    collection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

    splitform.Controls.Add(collection);

    var result = splitform.ShowDialog();

    for (int filenr = 0; filenr<breakpoints.Length; filenr++) {
      for (int funcnr = 0; funcnr < breakpoints[filenr].Length; funcnr++) {
        for (int rulenr = 0; rulenr < breakpoints[filenr][funcnr].Length; rulenr++) {
          for (int premnr = 0; premnr < breakpoints[filenr][funcnr][rulenr].Length; premnr++) {
            breakpoints[filenr][funcnr][rulenr][premnr] = program_tree.Nodes[filenr].Nodes[funcnr].Nodes[rulenr].Nodes[premnr].Checked;
          }
        }
      }
    }

    switch (result) {
      /*case System.Windows.Forms.DialogResult.OK: // step in
        return;
      case System.Windows.Forms.DialogResult.Yes: // step over
        return; */
      case System.Windows.Forms.DialogResult.Ignore: // continue
        return;
      case System.Windows.Forms.DialogResult.Abort:
        System.Environment.Exit(0);
        return;
    }
  }
}