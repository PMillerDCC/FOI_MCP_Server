using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DCC_Standard
{

	public class ViewBase : UserControl
    {

        public virtual void OnLoad(object sender, RoutedEventArgs e)
        {
            ErrorContainer = (IValidationErrorContainer)DataContext;
            AddHandler(Validation.ErrorEvent, new RoutedEventHandler(Handler), true);
        }

        public virtual void OnUnload(object sender, RoutedEventArgs e)
        {
            RemoveHandler(Validation.ErrorEvent, new RoutedEventHandler(Handler));
        }

        internal IValidationErrorContainer ErrorContainer = null;
        
        public void Handler(object sender, RoutedEventArgs e)
        {
            ValidationErrorEventArgs args = e as ValidationErrorEventArgs;

            if (args.Error.RuleInError is ValidationRule)
            {
                if (ErrorContainer != null)
                {
                    // Only want to work with validation errors that are Exceptions because the business object has already recorded the business rule violations using IDataErrorInfo.
                    BindingExpression bindingExpression = args.Error.BindingInError as System.Windows.Data.BindingExpression;

                    string propertyName = bindingExpression.ParentBinding.Path.Path;
                    DependencyObject OriginalSource = args.OriginalSource as DependencyObject;

                    // Construct the error message.
                    string errorMessage = "";
                    ReadOnlyObservableCollection<System.Windows.Controls.ValidationError> errors = Validation.GetErrors(OriginalSource);
                    if (errors.Count > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(propertyName).Append(":");
                        System.Windows.Controls.ValidationError error = errors[errors.Count - 1];
                        {
                            if (error.Exception == null || error.Exception.InnerException == null)
                                builder.Append(error.ErrorContent.ToString());
                            else
                                builder.Append(error.Exception.InnerException.Message);
                        }
                        errorMessage = builder.ToString();
                    }

                    // Add or remove the validation error to the validation error collection.
                    StringBuilder errorID = new StringBuilder();
                    errorID.Append(args.Error.RuleInError.ToString());
                    if (args.Action == ValidationErrorEventAction.Added)
                        ErrorContainer.AddError(new ValidationError(propertyName, errorID.ToString(), errorMessage));
                    else if (args.Action == ValidationErrorEventAction.Removed)
                        ErrorContainer.RemoveError(propertyName, errorID.ToString());

                }

            }

        }

    }

}
