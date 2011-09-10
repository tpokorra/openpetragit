//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// manages a list of open Petra Forms and Petra Dialogs.
    ///
    /// @Comment Managing such a list is necessary for (1) being able to check at
    ///   application close time whether any forms are still open that have unsaved
    ///   data, (2) to manage whether multiple or only a single instance of a form
    ///   should be allowed to be opened.
    /// </summary>
    public class TFormsList : DictionaryBase
    {
        /// <summary>Key: child; value: parent; both can be either Delphi or Progress windows</summary>
        private SortedList WindowRelationship;

        /// <summary>todoComment</summary>
        public static TFormsList GFormsList = new TFormsList();

        /// <summary>todoComment</summary>
        public System.Windows.Forms.Form this[string AKey]
        {
            get
            {
                System.Windows.Forms.Form ReturnValue;
                IDictionaryEnumerator DictEnum;
                ReturnValue = null;
                DictEnum = GetEnumerator();

                while (DictEnum.MoveNext())
                {
                    // MessageBox.Show('GetForm: DictEnum.Key: ' + DictEnum.Key.ToString);
                    if (DictEnum.Key.ToString().StartsWith(AKey))
                    {
                        ReturnValue = (System.Windows.Forms.Form)DictEnum.Value;
                        return ReturnValue;
                    }
                }

                return ReturnValue;
            }

            set
            {
                IDictionaryEnumerator DictEnum;
                DictEnum = GetEnumerator();

                while (DictEnum.MoveNext())
                {
                    // MessageBox.Show('GetForm: DictEnum.Key: ' + DictEnum.Key.ToString);
                    if (DictEnum.Key.ToString().StartsWith(AKey))
                    {
                        Dictionary[DictEnum.Value] = value;
                        return;
                    }
                }
            }
        }

        /// <summary>todoComment</summary>
        public ICollection Keys
        {
            get
            {
                return Dictionary.Keys;
            }
        }

        /// <summary>todoComment</summary>
        public ICollection Values
        {
            get
            {
                return Dictionary.Values;
            }
        }

        #region TFormsList

        /// <summary>
        /// constructor
        /// </summary>
        public TFormsList() : base()
        {
            WindowRelationship = new SortedList();
        }

        /// <summary>
        /// Type checking events
        /// </summary>
        /// <returns>void</returns>
        protected override void OnInsert(System.Object AKey, System.Object AValue)
        {
            VerifyType(AValue);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected override void OnSet(System.Object AKey, System.Object OldValue, System.Object NewValue)
        {
            VerifyType(NewValue);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="AValue"></param>
        protected override void OnValidate(System.Object AKey, System.Object AValue)
        {
            VerifyType(AValue);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AValue"></param>
        public void Add(System.Windows.Forms.Form AValue)
        {
            // MessageBox.Show('Adding Form ''' + GetKeyForForm(AValue).ToString + '''');
            Dictionary.Add(GetKeyForForm(AValue), AValue);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AValue"></param>
        public void Remove(System.Windows.Forms.Form AValue)
        {
            if (AValue != null)
            {
                // MessageBox.Show('Removing Form ''' + AValue.ToString + '''');
                Dictionary.Remove(GetKeyForForm(AValue));
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ANonClosableForms"></param>
        /// <param name="AFirstNonCloseableFormKey"></param>
        /// <returns></returns>
        public Boolean CanCloseAll(out StringCollection ANonClosableForms, out String AFirstNonCloseableFormKey)
        {
            System.Windows.Forms.Form FormInstance;
            IDictionaryEnumerator DictEnum;
            String NonCloseableForm;
            ANonClosableForms = new StringCollection();
            AFirstNonCloseableFormKey = "";
            DictEnum = GetEnumerator();

            while (DictEnum.MoveNext())
            {
                FormInstance = (System.Windows.Forms.Form)DictEnum.Value;

                if (FormInstance is IFrmPetraEdit)
                {
                    if (!((IFrmPetraEdit)FormInstance).CanClose())
                    {
                        NonCloseableForm = FormInstance.Text;

                        if (((TFrmPetraEditUtils)((IFrmPetraEdit)FormInstance).GetPetraUtilsObject()).HasChanges)
                        {
                            NonCloseableForm = NonCloseableForm + PetraEditForm.FORM_CHANGEDDATAINDICATOR;
                        }

                        ANonClosableForms.Add(NonCloseableForm);

                        if (ANonClosableForms.Count == 1)
                        {
                            AFirstNonCloseableFormKey = DictEnum.Key.ToString();
                        }
                    }
                }
            }

            return ANonClosableForms.Count == 0;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Boolean CloseAll()
        {
            return CloseAllExceptOne(null);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADontCloseForm"></param>
        /// <returns></returns>
        public Boolean CloseAllExceptOne(System.Windows.Forms.Form ADontCloseForm)
        {
            System.Windows.Forms.Form FormInstance;
            IDictionaryEnumerator DictEnum;
            System.Object OldKey;
            try
            {
                DictEnum = GetEnumerator();

                while (DictEnum.MoveNext())
                {
                    FormInstance = (System.Windows.Forms.Form)DictEnum.Value;
                    OldKey = DictEnum.Key;

                    if ((ADontCloseForm == null) || (FormInstance.GetType() != ADontCloseForm.GetType()))
                    {
                        // MessageBox.Show('ADontCloseForm.GetType: ' + ADontCloseForm.GetType().ToString);
                        // MessageBox.Show('About to close Form: ' + FormInstance.GetType().ToString + '; Title: ' + FormInstance.Text);
                        FormInstance.Close();

                        // make sure the form really has been removed; otherwise it could be an endless loop
                        if (Dictionary.Contains(OldKey) == true)
                        {
                            Dictionary.Remove(OldKey);
                        }

                        /*
                         * Need to go back to the beginning of the enumerator because the Enumerator won't work
                         * anymore after a Form has been closed (when a Form closes it removes
                         * itself from the FormsList)!
                         */
                        DictEnum = GetEnumerator();
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show("TFormsList.CloseAll: Exception occured while trying to close forms: " + Exp.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AFormName"></param>
        /// <returns></returns>
        public Boolean Contains(String AFormName)
        {
            System.Windows.Forms.Form FormInstance;
            IDictionaryEnumerator DictEnum;
            DictEnum = GetEnumerator();

            while (DictEnum.MoveNext())
            {
                FormInstance = (System.Windows.Forms.Form)DictEnum.Value;

                if (FormInstance.GetType().ToString().StartsWith(AFormName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AValue"></param>
        /// <returns></returns>
        public Boolean Contains(System.Windows.Forms.Form AValue)
        {
            System.Windows.Forms.Form FormInstance;
            IDictionaryEnumerator DictEnum;
            DictEnum = GetEnumerator();

            while (DictEnum.MoveNext())
            {
                FormInstance = (System.Windows.Forms.Form)DictEnum.Value;

                if (FormInstance.GetType().ToString().StartsWith(AValue.Text))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public String OpenForms()
        {
            String ReturnValue = "";
            const String PrintFormat = "Form Name: {0}, Form Title: {1}";

            System.Windows.Forms.Form FormInstance;
            IDictionaryEnumerator DictEnum;
            DictEnum = GetEnumerator();

            while (DictEnum.MoveNext())
            {
                FormInstance = (System.Windows.Forms.Form)DictEnum.Value;
                ReturnValue = ReturnValue + String.Format(PrintFormat, new Object[] { FormInstance.Name, FormInstance.Text }) + "\r\n";
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATitle"></param>
        /// <returns></returns>
        private System.Windows.Forms.Form GetFormByTitle(String ATitle)
        {
            System.Windows.Forms.Form FormInstance;
            IDictionaryEnumerator DictEnum;
            DictEnum = GetEnumerator();

            while (DictEnum.MoveNext())
            {
                FormInstance = (System.Windows.Forms.Form)DictEnum.Value;

                // MessageBox.Show('GetForm: FormInstance: ' + FormInstance.GetType().ToString);
                if (FormInstance.Text.StartsWith(ATitle))
                {
                    return FormInstance;
                }
            }

            return null;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AWindowHandle"></param>
        /// <returns></returns>
        public System.Windows.Forms.Form GetFormByHandle(IntPtr AWindowHandle)
        {
            System.Windows.Forms.Form FormInstance;
            IDictionaryEnumerator DictEnum;
            DictEnum = GetEnumerator();

            while (DictEnum.MoveNext())
            {
                FormInstance = (System.Windows.Forms.Form)DictEnum.Value;

                if (!FormInstance.IsDisposed)
                {
                    if (FormInstance.Handle == AWindowHandle)
                    {
                        return FormInstance;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AFormName"></param>
        /// <returns></returns>
        public Boolean ShowForm(String AFormName)
        {
            Boolean ReturnValue;

            System.Windows.Forms.Form TheForm;

            // MessageBox.Show('TFormsList.ShowForm called for Form named ''' + AFormName + '''');
            TheForm = this[AFormName];

            if (TheForm != null)
            {
                // MessageBox.Show('TFormsList.ShowForm: found Form, now activing it.');
                TheForm.Visible = true;
                TheForm.WindowState = FormWindowState.Normal;
                TheForm.Activate();
                ReturnValue = true;
            }
            else
            {
                // MessageBox.Show('TFormsList.ShowForm: Form NOT found!');
                ReturnValue = false;
            }

            return ReturnValue;
        }

        private System.Object GetKeyForForm(System.Windows.Forms.Form AForm)
        {
            System.Object ReturnValue;
            try
            {
                ReturnValue = (System.Object)(AForm.GetType().ToString() + '_' + AForm.Handle.ToString());
            }
            catch (System.ObjectDisposedException)
            {
                // In case where the Form is already disposed by Windows we can't access
                // its Handle anymore, so just return the Type in this case.
                ReturnValue = AForm.GetType().ToString();
#if DEBUGMODE
                MessageBox.Show(
                    "TFormsList.GetKeyForForm: Form of Type '" + ReturnValue.ToString() + "' is already Disposed!" + "\r\n" +
                    "This is a programmer error - a Form needs to be closed and removed from the FormsList before it may be Disposed!!!" + "\r\n" +
                    "(Disposing is also only necessary for Forms shown with ShowDialog.)", "DEVELOPER MESSAGE");
#endif
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }

        private void VerifyType(System.Object AValue)
        {
            // if not (AValue is System.Windows.Forms.Form) then
            // raise ArgumentException.Create('Invalid Type: TFormsList can only store Form Objects');
        }

        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AClosingWindowHandle"></param>
        public void NotifyWindowClose(IntPtr AClosingWindowHandle)
        {
            Int32 keyIndex = WindowRelationship.IndexOfKey((System.Object)AClosingWindowHandle.ToInt64());

            if (keyIndex != -1)
            {
                IntPtr callerHandle = (IntPtr)((Int64)WindowRelationship.GetByIndex(keyIndex));

                // MessageBox.Show('NotifyWindowClose:  AClosingWindowHandle: ' + AClosingWindowHandle.ToString + '; callerHandle:' + callerHandle.ToString);
                // check whether the calling window is Delphi or Progress
                // System.Windows.Forms.Form callerForm = GetFormByHandle(callerHandle);

                // set focus to the caller window
                WindowHandling.SetForegroundWindowWrapper(callerHandle);
                WindowHandling.ShowWindowWrapper(callerHandle, WindowHandling.SW_RESTORE);

                // remove from list
                WindowRelationship.Remove((System.Object)AClosingWindowHandle.ToInt64());
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACallerWindowHandle"></param>
        /// <param name="ANewWindowHandle"></param>
        public void NotifyWindowOpened(IntPtr ACallerWindowHandle, IntPtr ANewWindowHandle)
        {
            // NewWindow is unique therefore stored as key, calling window can open several windows
            // except for the situation when NewWindow is actually an existing window, e.g. the Partner Main screen will only be opened once, and then reused
            if ((!WindowRelationship.Contains((System.Object)ANewWindowHandle.ToInt64())))
            {
                try
                {
                    // MessageBox.Show('NotifyWindowOpened:  ANewWindowHandle: ' + ANewWindowHandle.ToString + '; ACallerWindowHandle: ' + ACallerWindowHandle.ToString);
                    WindowRelationship.Add((System.Object)ANewWindowHandle.ToInt64(), (System.Object)ACallerWindowHandle.ToInt64());
                }
                catch (ArgumentException)
                {
                    // http://bugs.om.org/petra/show_bug.cgi?id=769
                    // sometimes we still get an exception in the logs; not sure why...
                    // seems our contains check above does not work; exception complains that there is already such a key
                }
            }
        }
    }
}