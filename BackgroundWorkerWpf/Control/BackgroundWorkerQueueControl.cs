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

namespace ReactiveAsyncWorkerWpf
{




    public class BackgroundWorkerQueueControl : AsyncWorkerQueueControl<IWorkerItem<object>,object>
    {
        static BackgroundWorkerQueueControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BackgroundWorkerQueueControl), new FrameworkPropertyMetadata(typeof(BackgroundWorkerQueueControl)));

        }
        
        public BackgroundWorkerQueueControl()
        {

            Uri resourceLocater = new Uri("/ReactiveAsyncWorkerWpf;component/Themes/BackgroundQueueWorkerControl.xaml", System.UriKind.Relative);
            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            Style = resourceDictionary["BackgroundWorkerQueueControlStyle"] as Style;
        }

        protected override UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason,IWorkerItem< object>>> NewItemsInitialise(IObservable<object> newitems)
        {
            return new BackgroundWorkerCommandQueue<object>(newitems.Select(_=>(WorkerArgument<object>)_));
        }
    }
}
