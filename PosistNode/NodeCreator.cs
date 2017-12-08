using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PosistNode.ViewModel;

namespace PosistNode
{
    public partial class NodeCreator : Form
    {
        public NodeCreator()
        {
            InitializeComponent();
        }

        private NodeManagerVM _vm;

        private void NodeCreator_Load(object sender, EventArgs e)
        {
            this._vm = new NodeManagerVM();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
