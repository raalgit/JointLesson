using JointLessonTerminal.Core;
using JointLessonTerminal.Core.Material;
using JointLessonTerminal.Model.ServerModels;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.EventModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class EditorWindowViewModel : ObservableObject
    {
        #region открытые поля
        public ManualData ManualData { get { return manualData; } set { manualData = value; OnPropsChanged("ManualData"); } }
        public ObservableCollection<Manual> MyManuals { get { return myManuals; } set { myManuals = value; OnPropsChanged("MyManuals"); } }
        public Manual SelectedManual
        {
            get { return selectedManual; }
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
        public bool UpdateMod
        {
            get { return updateMod; }
            set { updateMod = value; OnPropsChanged("UpdateMod"); }
        }
        public bool CreateMod
        {
            get { return createMod; }
            set { createMod = value; OnPropsChanged("CreateMod"); }
        }
        public RelayCommand CreateNewManualCommand { get; set; }
        public RelayCommand UpdateManualCommand { get; set; }
        #endregion

        #region закрытые поля
        private MaterialHandler materialHandler;
        private UserSettings userSettings;
        private ManualData manualData;
        private ObservableCollection<Manual> myManuals;
        private Manual selectedManual;
        private bool updateMod;
        private bool createMod;
        #endregion

        public EditorWindowViewModel()
        {

        }

        #region открытые методы
        public void InitData()
        {
            userSettings = UserSettings.GetInstance();
            materialHandler = new MaterialHandler();

            UpdateMod = false;
            CreateMod = true;

            Task.Factory.StartNew(async () => {
                await loadMyMaterials();
            });

            /// Шаблонная запись метариала
            ManualData = new ManualData()
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
                id = Guid.NewGuid().ToString(),
                chapters = new ObservableCollection<Chapter>()
            };

            CreateNewManualCommand = new RelayCommand(async x =>
            {
                bool resp = await materialHandler.SaveAtDataBase(manualData);
                if (resp) await loadMyMaterials();
            });
            UpdateManualCommand = new RelayCommand(async x =>
            {
                bool resp = await materialHandler.UpdateAtDataBase(manualData, selectedManual.fileDataId.Value);
            });
        }
        #endregion

        #region закрытые методы
        private async Task loadMyMaterials()
        {
            MyManuals = await materialHandler.GetMyMaterials();
        }
        private async Task loadMaterialData(Manual manual)
        {
            if (manual == null) return;
            CreateMod = false;
            UpdateMod = true;
            ManualData = await materialHandler.LoadById(manual.fileDataId.Value);
        }
        #endregion
    }
}
