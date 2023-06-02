﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ProjectLink : ViewModelBase
    {
        private int _linkID;
        private int _projectID;
        private string _pathDesc;
        private string _pathName;

        public int LinkID
        {
            get => _linkID;
            set
            {
                if (value == _linkID) return;
                _linkID = value;
                OnPropertyChanged();
            }
        }

        public int ProjectID
        {
            get => _projectID;
            set
            {
                if (value == _projectID) return;
                _projectID = value;
                OnPropertyChanged();
            }
        }

        public string PathDesc
        {
            get => _pathDesc;
            set
            {
                if (value == _pathDesc) return;
                _pathDesc = value;
                OnPropertyChanged();
            }
        }

        public string PathName
        {
            get => _pathName;
            set
            {
                if (value == _pathName) return;
                _pathName = value;
                OnPropertyChanged();
            }
        }
    }
}