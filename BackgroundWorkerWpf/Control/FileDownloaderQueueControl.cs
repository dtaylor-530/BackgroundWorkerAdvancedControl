using ReactiveAsyncWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

//using DynamicData;

using System.Collections;
using UtilityWpf;
using System.Windows.Markup;
using UtilityWpf.View;
using Reactive.Bindings;
using UtilityHelper;

namespace ReactiveAsyncWorkerWpf
{




    public class FileDownloaderQueueControl : AsyncWorkerQueueControl<IWorkerItem<object>,object>
    {

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(FileDownloaderQueueControl), new PropertyMetadata("Path"));
        public static readonly DependencyProperty UriProperty = DependencyProperty.Register("Uri", typeof(string), typeof(FileDownloaderQueueControl), new PropertyMetadata("Uri"));


        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }


        static FileDownloaderQueueControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileDownloaderQueueControl), new FrameworkPropertyMetadata(typeof(FileDownloaderQueueControl)));
        }



        public FileDownloaderQueueControl()
        {

            Uri resourceLocater = new Uri("/ReactiveAsyncWorkerWpf;component/Themes/FileDownloaderQueueControl.xaml", System.UriKind.Relative);
            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            Style = resourceDictionary["FileDownloaderQueueControlStyle"] as Style;
        }


        protected override UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason, IWorkerItem<object>>> NewItemsInitialise(IObservable<object> newitems)
        {
            Check(newitems);
            var nis=NewItemSubject.Select(_ =>
            {
                var uri = _.GetPropValue<Uri>(Uri);
                var path = _.GetPropValue<string>(Path);
                return Tuple.Create(uri, path);
            });
            return new FileDownloaderCommandQueue(nis);

        }


        private void Check(IObservable<object> items)
        {

            items.Take(1).Subscribe(_ =>
            {
                var props = _.GetType().GetProperties();

                if (!props.Select(_a => _a.Name).Contains(Uri))
                    throw new Exception(nameof(Uri) + " Property has not been provided to " + nameof(FileDownloaderQueueControl));
                if (!props.Select(_a => _a.Name).Contains(Path))
                    throw new Exception(nameof(Path) + " Property has not been provided to " + nameof(FileDownloaderQueueControl));

                //return _;

            });

        }
    }

}
