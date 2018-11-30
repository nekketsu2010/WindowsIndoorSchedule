using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 授業用ツール
{
    class DocumentClass
    {
        private string documentName; //ファイルの名前
        private string documentPass; //相対ファイルパス
        private bool open = true; //自動オープンするか（デフォルトでTrue）

        public DocumentClass()
        {

        }

        public void setDocumentName(string documentName)
        {
            this.documentName = documentName;
        }
        public string getDocumentName()
        {
            return documentName;
        }

        public void setDocumentPass(string pass)
        {
            this.documentPass = pass;
        }
        public string getDocumentPass()
        {
            return documentPass;
        }

        public void setOpen(bool open)
        {
            this.open = open;
        }
        public bool getOpen()
        {
            return open;
        }
    }
}
