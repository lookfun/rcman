﻿using RemoteConnectionManager.ViewModels;
using RemoteConnectionManager.ViewModels.Properties;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RemoteConnectionManager.Controls
{
    public class CategoryTreeViewItem : TreeViewItem
    {
        public CategoryTreeViewItem() : base()
        {
            IsHitTestVisible = true;
            AllowDrop = true;

            Loaded += CategoryTreeViewItem_Loaded;
        }

        private void CategoryTreeViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= CategoryTreeViewItem_Loaded;

            var itemsControl = ItemsControlFromItemContainer(this);

            WeakEventManager<ItemContainerGenerator, ItemsChangedEventArgs>.AddHandler(
                itemsControl.ItemContainerGenerator,
                "ItemsChanged",
                (sender1, e1) => SetExtender());

            SetExtender();
        }

        public bool UseExtender
        {
            get { return (bool)GetValue(UseExtenderProperty); }
            set { SetValue(UseExtenderProperty, value); }
        }

        public static readonly DependencyProperty UseExtenderProperty = DependencyProperty.Register(
            "UseExtender", typeof(bool),
            typeof(CategoryTreeViewItem),
            new PropertyMetadata(false));

        private void SetExtender()
        {
            var itemsControl = ItemsControlFromItemContainer(this);
            if (itemsControl != null)
            {
                SetValue(UseExtenderProperty, itemsControl.ItemContainerGenerator.IndexFromContainer(this) == itemsControl.Items.Count - 1);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CategoryTreeViewItem();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Mouse.LeftButton == MouseButtonState.Pressed && DataContext != null)
            {
                DragDrop.DoDragDrop(this, DataContext, DragDropEffects.Move);
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            var dropTarget = DataContext as CategoryItemViewModel;
            if (e.Data.GetDataPresent(typeof(CategoryItemViewModel)))
            {
                var dragSource = (CategoryItemViewModel)e.Data.GetData(typeof(CategoryItemViewModel));
                if (dragSource != dropTarget && dropTarget.Properties is GenericPropertiesViewModel)
                {
                    ViewModelLocator.Locator.DragDrop.Drop(dragSource, dropTarget);
                }
            }

            e.Handled = true;
        }
    }
}
