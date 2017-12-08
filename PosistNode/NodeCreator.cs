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
            this.clearNewNode();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdCreate_Click(object sender, EventArgs e)
        {
            this.clearNewNode();
        }

        private void clearNewNode()
        {
            this.nametxt.Text = String.Empty;
            this.addressTxt.Text = String.Empty;
            this.mobileTxt.Text = String.Empty;
            this.phoneTxt.Text = String.Empty;
            this.valueTxt.Text = String.Empty;
            this.setTxt.Text = string.Empty;
            this.algoKeyLbl.Text = "None";
            this.saveIdLbl.Text = "None";
            //reset the display node to show nothing.
            this._vm.DisplayNode = null;
            this.Mode = Constants.Mode.Add;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            //dont resave the same node again.
            if(this.Mode == Constants.Mode.Edit)
            {
                return;
            }
            if(!this.validateCreateFields())
            {
                return;
            }
            int set = 0;
            if(!int.TryParse(this.setTxt.Text,out set))
            {
                set = -1;
            }
            this.saveIdLbl.Text = this._vm.CreateNew(this.nametxt.Text, this.addressTxt.Text, this.mobileTxt.Text, this.phoneTxt.Text,
                                                     float.Parse(this.valueTxt.Text), this.passText.Text,set).ToString();
            this.algoKeyLbl.Text = this._vm.DisplayNode.Cipher.Key;
            this.Mode = Constants.Mode.Edit;
        }

        private bool validateCreateFields()
        {
            bool pass = true;
            double garbage;
            if(String.IsNullOrWhiteSpace(this.nametxt.Text))
            {
                pass = false;
                this.nametxt.Focus();
            }
            if (String.IsNullOrWhiteSpace(this.addressTxt.Text))
            {
                pass = false;
                this.addressTxt.Focus();
            }
            if (String.IsNullOrWhiteSpace(this.mobileTxt.Text) || !Double.TryParse(this.mobileTxt.Text, out garbage))
            {
                pass = false;
                this.mobileTxt.Focus();
            }
            if (String.IsNullOrWhiteSpace(this.phoneTxt.Text) || !Double.TryParse(this.phoneTxt.Text, out garbage))
            {
                pass = false;
                this.phoneTxt.Focus();
            }
            if (String.IsNullOrWhiteSpace(this.valueTxt.Text) || !Double.TryParse(this.valueTxt.Text,out garbage))
            {
                pass = false;
                this.valueTxt.Focus();
            }
            if (String.IsNullOrWhiteSpace(this.passText.Text))
            {
                pass = false;
                this.passText.Focus();
            }
            if (!pass)
            {
                MessageBox.Show("Empty/Invalid Fields", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return pass;
        }

        private void verifyNodeCmd_Click(object sender, EventArgs e)
        {
            if(!this.validateForVerify())
            {
                return;
            }
            bool isEmpty = this._vm.DisplayNode == null;
            if(this._vm.VerifyNode(this.passText.Text, this.algoKeyTxt.Text))
            {
                MessageBox.Show("Verified!", "Verification", MessageBoxButtons.OK);
                if(isEmpty)
                {
                    this.fillNode(this._vm.DisplayNode);
                }
            }
            else{
                MessageBox.Show("Not Verified!", "Verification", MessageBoxButtons.OK);
            }
            
        }

        private void fillNode(App_Code.Node node)
        {
            if(node == null)
            {
                return;
            }
            Dictionary<string, string> data = node.GetData();
            
            this.nametxt.Text = data.ContainsKey("name") ? data["name"] : "";
            this.addressTxt.Text = data.ContainsKey("address") ? data["address"] : "";
            this.mobileTxt.Text = data.ContainsKey("mobile") ? data["mobile"] : "";
            this.phoneTxt.Text = data.ContainsKey("phone") ? data["phone"] : "";
            this.valueTxt.Text = data.ContainsKey("value") ? data["value"] : "";
            this.setTxt.Text = node.NodeNumber.ToString();
            this.algoKeyLbl.Text = node.Cipher.Key;
            this.saveIdLbl.Text = node.Id.ToString();
            this.passText.Text = node.Cipher.Password;
            this.Mode = Constants.Mode.Edit;
            if(node.ParentNode != null)
            {
                this.parentNodeIdTxt.Text = node.ParentNode.Id.ToString();
            }
            else
            {
                this.parentNodeIdTxt.Text = "First Node";
            }
            if(node.ChildNode != null)
            {
                this.childNodeIdTxt.Text = node.ChildNode.Id.ToString();
            }
        }

        private bool validateForVerify()
        {
            bool pass = true;
            if (String.IsNullOrWhiteSpace(this.algoKeyTxt.Text))
            {
                pass = false;
                this.algoKeyTxt.Focus();
            }
            if (String.IsNullOrWhiteSpace(this.passText.Text))
            {
                pass = false;
                this.passText.Focus();
            }
            if (!pass)
            {
                MessageBox.Show("Empty/Invalid Fields", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return pass;
        }

        private void getNodeBtn_Click(object sender, EventArgs e)
        {
            long nodeId;
            if(!long.TryParse(this.nodeIdTxt.Text,out nodeId))
            {
                this.nodeIdTxt.Focus();
                MessageBox.Show("Enter a Valid Node Id");
                return;
            }
            if(!this._vm.FindNode(nodeId))
            {
                MessageBox.Show("Node Not Found");
                return;
            }
            this.fillNode(this._vm.DisplayNode);
            this.Mode = Constants.Mode.Edit;
        }

        private Constants.Mode Mode
        {
            get
            {
                return this._vm.Mode;
            }
            set
            {
                switch (value)
                {
                    case Constants.Mode.Add:
                        editGb.Enabled = false;
                        newNodeGb.Enabled = true;
                        modeLbl.Text = "Add";
                        break;
                    case Constants.Mode.Edit:
                        editGb.Enabled = true;
                        newNodeGb.Enabled = false;
                        modeLbl.Text = "Edit";
                        break;
                }
                this._vm.Mode = value;
            }
        }

        private void mergeBtn_Click(object sender, EventArgs e)
        {
            if(!this.validateForMerge())
            {
                return;
            }
            if(!this._vm.MergeSets(int.Parse(this.set1Txt.Text), int.Parse(this.set2Txt.Text)))
            {
                MessageBox.Show("Error - " + this._vm.LastError);
            }
            else
            {
                MessageBox.Show("Successfully Merged");
            }
        }

        private bool validateForMerge()
        {
            bool pass = true;
            int garbage;
            if (String.IsNullOrWhiteSpace(this.set1Txt.Text) || !int.TryParse(this.set1Txt.Text,out garbage))
            {
                pass = false;
                this.set1Txt.Focus();
            }
            if (String.IsNullOrWhiteSpace(this.set2Txt.Text) || !int.TryParse(this.set2Txt.Text, out garbage))
            {
                pass = false;
                this.set2Txt.Focus();
            }
            if (!pass)
            {
                MessageBox.Show("Empty/Invalid Fields", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return pass;
        }

        private void transferBtn_Click(object sender, EventArgs e)
        {
            if(!this.validateForIntegers(this.tranferNodeIdTxt,this.transferParentNodeId) || !this.validateForVerify())
            {
                return;
            }
            if(!this._vm.Transfer(int.Parse(this.tranferNodeIdTxt.Text), int.Parse(this.transferParentNodeId.Text),this.passText.Text,this.algoKeyTxt.Text))
            {
                MessageBox.Show("Tranfer Failed - " + this._vm.LastError);
            }
            else
            {
                MessageBox.Show("Tranfer Success");
            }
        }

        private bool validateForIntegers(params TextBox[] box)
        {
            bool pass = true;
            int garbage;
            if(box.Length < 1)
            {
                return true;
            }
            foreach(TextBox tb in box)
            {
                if (String.IsNullOrWhiteSpace(tb.Text) || !int.TryParse(tb.Text, out garbage))
                {
                    pass = false;
                    tb.Focus();
                }
            }
            return pass;
        }

        private void subChainLengthCmd_Click(object sender, EventArgs e)
        {
            this.subChainLengthTxt.Text = (this._vm.DisplayNode == null) ? "None" : this._vm.DisplayNode.ChainLength.ToString();
        }

        private void cmdBreakNode_Click(object sender, EventArgs e)
        {
            if(!this.validateForIntegers(this.breakValuetxt))
            {
                return;
            }
            if(this._vm.DisplayNode == null)
            {
                MessageBox.Show("Load a Node first!");
                this.Mode = Constants.Mode.Add;
                return;
            }else{
                this._vm.DisplayNode.BreakUpNode(int.Parse(breakValuetxt.Text));
                this.fillNode(this._vm.DisplayNode.ChildNode);
                MessageBox.Show("Node Broken, Broken node is shown in Create Node Section!");
            }
        }

        private void algoKeyTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void maxChain_Click(object sender, EventArgs e)
        {
            this.maxChainLenTxt.Text = this._vm.MaxChainLength().ToString();
        }

        private void algoKeyLbl_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyData == (Keys.Control | Keys.ControlKey))
            {
                return;
            }
            if(!(e.Control &&  e.KeyData == (Keys.C | Keys.Control)))
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
    }
}
