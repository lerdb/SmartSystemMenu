﻿using System;

namespace SmartSystemMenu.Hooks
{
    class KeyboardLLHook : Hook
    {
        private int msgID_KeyboardLL;
        private int msgID_KeyboardLL_HookReplaced;

        public event EventHandler<EventArgs> HookReplaced;
        public event EventHandler<BasicHookEventArgs> KeyboardLLEvent;

        public KeyboardLLHook(IntPtr handle) : base(handle)
        {
        }

        protected override void OnStart()
        {
            msgID_KeyboardLL = NativeMethods.RegisterWindowMessage("SMART_SYSTEM_MENU_HOOK_KEYBOARDLL");
            msgID_KeyboardLL_HookReplaced = NativeMethods.RegisterWindowMessage("SMART_SYSTEM_MENU_HOOK_KEYBOARDLL_REPLACED");

            if (Environment.OSVersion.Version.Major >= 6)
            {
                NativeMethods.ChangeWindowMessageFilter(msgID_KeyboardLL, NativeConstants.MSGFLT_ADD);
                NativeMethods.ChangeWindowMessageFilter(msgID_KeyboardLL_HookReplaced, NativeConstants.MSGFLT_ADD);
            }
            NativeHookMethods.InitializeKeyboardLLHook(0, handle);
        }

        protected override void OnStop()
        {
            NativeHookMethods.UninitializeKeyboardLLHook();
        }

        public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == msgID_KeyboardLL)
            {
                RaiseEvent(KeyboardLLEvent, new BasicHookEventArgs(m.WParam, m.LParam));
            }
            else if (m.Msg == msgID_KeyboardLL_HookReplaced)
            {
                RaiseEvent(HookReplaced, EventArgs.Empty);
            }
        }
    }
}