using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LitSupport
{
    class TableRecord
    {
        private string docID;
        private string to;
        private string from;
        private string cc;
        private string bcc;

        public TableRecord(string docID, string to, string from, string cc, string bcc)
        {
            this.docID = docID;
            this.to = to;
            this.from = from;
            this.cc = cc;
            this.bcc = bcc;
        }

        public TableRecord(string docID, string to, string from, string cc)
        {
            this.docID = docID;
            this.to = to;
            this.from = from;
            this.cc = cc;
            this.bcc = null;
        }

        public TableRecord(string docID, string to, string from)
        {
            this.docID = docID;
            this.to = to;
            this.from = from;
            this.cc = null;
            this.bcc = null;
        }

        public string DocID
        {
            get { return this.docID; }
        }
        public string To
        {
            get { return this.to; }
        }
        public string From
        {
            get { return this.from; }
        }
        public string CC
        {
            get { return this.cc; }
        }
        public string BCC
        {
            get { return this.bcc; }
        }
    }
}
