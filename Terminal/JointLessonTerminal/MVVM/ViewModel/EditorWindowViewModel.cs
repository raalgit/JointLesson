using JointLessonTerminal.Core;
using JointLessonTerminal.Core.Material;
using JointLessonTerminal.Model.ServerModels;
using JointLessonTerminal.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class EditorWindowViewModel : ObservableObject
    {
        private ManualData manualData;
        public ManualData ManualData
        {
            get { return manualData; }
            set { 
                manualData = value;
                OnPropsChanged("ManualData");
            }
        }

        private List<Manual> myManuals;
        public List<Manual> MyManuals {
            get
            { 
                return myManuals;
            } 
            set {
                myManuals = value;
                OnPropsChanged("MyManuals");
            }
        }

        private Manual selectedManual;
        public Manual SelectedManual 
        {
            get 
            { 
                return selectedManual; 
            }
            set
            {
                selectedManual = value;
                if (selectedManual != null && selectedManual.fileDataId.HasValue)
                {
                    Task.Factory.StartNew(async x =>
                    {
                        await loadMaterialData(SelectedManual);
                    }, null);
                }
            }
        }

        private MaterialHandler materialHandler;
        private UserSettings userSettings;

        private bool updateMod;
        public bool UpdateMod
        {
            get
            {
                return updateMod;
            }
            set
            {
                updateMod = value;
                OnPropsChanged("UpdateMod");
            }
        }

        private bool createMod;
        public bool CreateMod
        {
            get
            {
                return createMod;
            }
            set
            {
                createMod = value;
                OnPropsChanged("CreateMod");
            }
        }

        public RelayCommand CreateNewManualCommand { get; set; }
        public RelayCommand UpdateManualCommand { get; set; }

        public EditorWindowViewModel()
        {

        }

        public void InitData()
        {
            userSettings = UserSettings.GetInstance();
            materialHandler = new MaterialHandler();

            UpdateMod = false;
            CreateMod = true;

            Task.Factory.StartNew(() => loadMyMaterials());

            manualData = new ManualData()
            {
                authors = new List<Author>()
                {
                    new Author()
                    {
                        name = userSettings.CurrentUser.firstName + " " + userSettings.CurrentUser.secondName,
                        mail = "",
                        token = ""
                    }
                },
                access = 0,
                discipline = "Программная инженерия",
                name = "Материал программная инженерии",
                materialDate = new MaterialDate()
                {
                    created = DateTime.Now,
                    modified = DateTime.Now
                },
                number = 0,
                parts = 0,
                chapters = new List<Chapter>()
            };

            CreateNewManualCommand = new RelayCommand(x =>
            {
                materialHandler.SaveAtDataBase(manualData);
            });
            UpdateManualCommand = new RelayCommand(x =>
            {
                materialHandler.UpdateAtDataBase(manualData, selectedManual.fileDataId.Value);
            });
        }

        private async Task loadMyMaterials()
        {
            MyManuals = await materialHandler.GetMyMaterials();
        }

        private async Task loadMaterialData(Manual manual)
        {
            if (manual == null) return;
            CreateMod = false;
            UpdateMod = true;
            manualData = await materialHandler.LoadById(manual.fileDataId.Value);
        }
    }
}
