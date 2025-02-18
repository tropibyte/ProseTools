using com.sun.xml.@internal.txw2.output;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class AIQueryDlg : Form
    {
        public AIQueryDlg()
        {
            InitializeComponent();
        }

        // Assume your designer has:
        // - txtQuery: TextBox for user prompt.
        // - txtResults: Multiline TextBox (or RichTextBox) for the response.
        // - btnQuery: Button to execute the query.

        private async void btnQuery_Click(object sender, EventArgs e)
        {
            string prompt = txtQuery.Text.Trim();
            if (string.IsNullOrWhiteSpace(prompt))
            {
                MessageBox.Show("Please enter a query.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the current GenAI configuration.
            GenAIConfig config = Globals.ThisAddIn.GetCurrentGenAIConfig();
            if (string.IsNullOrWhiteSpace(config.Name))
            {
                // If an empty config is returned, exit.
                return;
            }

            // Optionally, disable controls while waiting.
            btnQuery.Enabled = false;
            txtResults.Text = "Querying, please wait...";

            try
            {
                // Call your GenAIService (assumed implemented similarly to your friend's sample).
                GenAIService service = new GenAIService();
                string response = await service.GetAIResponseAsync(prompt, config);
                txtResults.Text = response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during the query: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnQuery.Enabled = true;
            }
        }
    }
}
