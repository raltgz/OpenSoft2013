using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Citeseer
{
    /// <summary>
    /// Interaction logic for AuthorProfile.xaml
    /// </summary>
    public partial class AuthorProfile : Page
    {
        String name;
        public AuthorProfile()
        {
            InitializeComponent();
        }

        public void setName(String name)
        {
            authorName.Text = name;
            this.name = name;
        }

        public String getName()
        {
            return name;
        }

        public void setInstitute(String insti)
        {
            authorInstitute.Text = insti;
        }

        public void setCitations(int citations)
        {
            this.citations.Text = "" + citations;
        }

        public void setHIndex(int hindex)
        {
            this.hIndex.Text = "" + hindex;
        }

        public void setIIndex(int iindex)
        {
            this.iIndex.Text = "" + iindex;
        }

        public void setInterests(String interests)
        {
            if (interests != null)
                this.interests.Text = "" + interests;
            else
                this.interests.Text = "Unknown Interests";
        }

        public void setTotal(String total)
        {
            this.total.Text = "" + total;
        }

        public void setCitesPaper(String cites_paper)
        {
            this.cites_paper.Text = "" + cites_paper;
        }

        public void setPicture(Uri url)
        {
            webView.Navigate(url);
        }
    }
}
