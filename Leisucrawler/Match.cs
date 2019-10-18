using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leisucrawler
{
    public class Match
    {
        private string matchinfo;
        public string Matchinfo
        {
            get { return matchinfo; }
            set { matchinfo = value; }
        }
        private string matchscore;
        public string Matchscore
        {
            get { return matchscore; }
            set { matchscore = value; }
        }
        private string teaminfo;
        public string Teaminfo
        {
            get { return teaminfo; }
            set { teaminfo = value; }
        }
        private string matchtime;
        public string Matchtime
        {
            get { return matchtime; }
            set { matchtime = value; }
        }
        private string matchodds;
        public string Matchodds
        {
            get { return matchodds; }
            set { matchodds = value; }
        }

        private string smallodds;
        public string Smallodds
        {
            get { return smallodds; }
            set { smallodds = value; }
        }

        private string dataid;
        public string Dataid
        {
            get { return dataid; }
            set { dataid = value; }
        }

        private string scoretotal;
        public string Scoretotal
        {
            get { return scoretotal; }
            set { scoretotal = value; }
        }

        private string scorehalf;
        public string Scorehalf
        {
            get { return scorehalf; }
            set { scorehalf = value; }
        }

        private string ballcount;
        public string Ballcount
        {
            get { return ballcount; }
            set { ballcount = value; }
        }

        private string chupanballcount;
        public string Chupanballcount
        {
            get { return chupanballcount; }
            set { chupanballcount = value; }
        }
        private string goalslist;
        public string Goalslist
        {
            get { return goalslist; }
            set { goalslist = value; }
        }

        private string stats;
        public string Stats
        {
            get { return stats; }
            set { stats = value; }
        }
    }
}
