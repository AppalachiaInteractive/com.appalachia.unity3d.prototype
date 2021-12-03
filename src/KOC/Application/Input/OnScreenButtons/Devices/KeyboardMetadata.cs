using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Controls;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Devices
{
    [DoNotReorderFields]
    [Serializable, SmartLabelChildren, SmartLabel]
    public sealed class KeyboardMetadata : DeviceMetadata
    {
        #region Fields and Autoproperties

        public ControlButtonMetadata escape;
        public ControlButtonMetadata space;
        public ControlButtonMetadata enter;
        public ControlButtonMetadata tab;
        public ControlButtonMetadata backquote;
        public ControlButtonMetadata quote;
        public ControlButtonMetadata semicolon;
        public ControlButtonMetadata comma;
        public ControlButtonMetadata period;
        public ControlButtonMetadata slash;
        public ControlButtonMetadata backslash;
        public ControlButtonMetadata leftBracket;
        public ControlButtonMetadata rightBracket;
        public ControlButtonMetadata minus;
        public ControlButtonMetadata equals;
        public ControlButtonMetadata upArrow;
        public ControlButtonMetadata downArrow;
        public ControlButtonMetadata leftArrow;
        public ControlButtonMetadata rightArrow;
        public ControlButtonMetadata a;
        public ControlButtonMetadata b;
        public ControlButtonMetadata c;
        public ControlButtonMetadata d;
        public ControlButtonMetadata e;
        public ControlButtonMetadata f;
        public ControlButtonMetadata g;
        public ControlButtonMetadata h;
        public ControlButtonMetadata i;
        public ControlButtonMetadata j;
        public ControlButtonMetadata k;
        public ControlButtonMetadata l;
        public ControlButtonMetadata m;
        public ControlButtonMetadata n;
        public ControlButtonMetadata o;
        public ControlButtonMetadata p;
        public ControlButtonMetadata q;
        public ControlButtonMetadata r;
        public ControlButtonMetadata s;
        public ControlButtonMetadata t;
        public ControlButtonMetadata u;
        public ControlButtonMetadata v;
        public ControlButtonMetadata w;
        public ControlButtonMetadata x;
        public ControlButtonMetadata y;
        public ControlButtonMetadata z;
        public ControlButtonMetadata _1;
        public ControlButtonMetadata _2;
        public ControlButtonMetadata _3;
        public ControlButtonMetadata _4;
        public ControlButtonMetadata _5;
        public ControlButtonMetadata _6;
        public ControlButtonMetadata _7;
        public ControlButtonMetadata _8;
        public ControlButtonMetadata _9;
        public ControlButtonMetadata _0;
        public ControlButtonMetadata leftShift;
        public ControlButtonMetadata rightShift;
        public ControlButtonMetadata shift;
        public ControlButtonMetadata leftAlt;
        public ControlButtonMetadata rightAlt;
        public ControlButtonMetadata alt;
        public ControlButtonMetadata leftCtrl;
        public ControlButtonMetadata rightCtrl;
        public ControlButtonMetadata ctrl;
        public ControlButtonMetadata leftMeta;
        public ControlButtonMetadata rightMeta;
        public ControlButtonMetadata contextMenu;
        public ControlButtonMetadata backspace;
        public ControlButtonMetadata pageDown;
        public ControlButtonMetadata pageUp;
        public ControlButtonMetadata home;
        public ControlButtonMetadata end;
        public ControlButtonMetadata insert;
        public ControlButtonMetadata delete;
        public ControlButtonMetadata capsLock;
        public ControlButtonMetadata numLock;
        public ControlButtonMetadata printScreen;
        public ControlButtonMetadata scrollLock;
        public ControlButtonMetadata pause;
        public ControlButtonMetadata numpadEnter;
        public ControlButtonMetadata numpadDivide;
        public ControlButtonMetadata numpadMultiply;
        public ControlButtonMetadata numpadPlus;
        public ControlButtonMetadata numpadMinus;
        public ControlButtonMetadata numpadPeriod;
        public ControlButtonMetadata numpadEquals;
        public ControlButtonMetadata numpad1;
        public ControlButtonMetadata numpad2;
        public ControlButtonMetadata numpad3;
        public ControlButtonMetadata numpad4;
        public ControlButtonMetadata numpad5;
        public ControlButtonMetadata numpad6;
        public ControlButtonMetadata numpad7;
        public ControlButtonMetadata numpad8;
        public ControlButtonMetadata numpad9;
        public ControlButtonMetadata numpad0;
        public ControlButtonMetadata f1;
        public ControlButtonMetadata f2;
        public ControlButtonMetadata f3;
        public ControlButtonMetadata f4;
        public ControlButtonMetadata f5;
        public ControlButtonMetadata f6;
        public ControlButtonMetadata f7;
        public ControlButtonMetadata f8;
        public ControlButtonMetadata f9;
        public ControlButtonMetadata f10;
        public ControlButtonMetadata f11;
        public ControlButtonMetadata f12;
        public ControlButtonMetadata wasd;
        public ControlButtonMetadata arrowkeys;

        #endregion

        public override bool CanResolve(InputControl control)
        {
            switch (control.name)
            {
                case "escape":
                case "space":
                case "enter":
                case "tab":
                case "backquote":
                case "quote":
                case "semicolon":
                case "comma":
                case "period":
                case "slash":
                case "backslash":
                case "leftBracket":
                case "rightBracket":
                case "minus":
                case "equals":
                case "upArrow":
                case "downArrow":
                case "leftArrow":
                case "rightArrow":
                case "a":
                case "b":
                case "c":
                case "d":
                case "e":
                case "f":
                case "g":
                case "h":
                case "i":
                case "j":
                case "k":
                case "l":
                case "m":
                case "n":
                case "o":
                case "p":
                case "q":
                case "r":
                case "s":
                case "t":
                case "u":
                case "v":
                case "w":
                case "x":
                case "y":
                case "z":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                case "leftShift":
                case "rightShift":
                case "shift":
                case "leftAlt":
                case "rightAlt":
                case "alt":
                case "leftCtrl":
                case "rightCtrl":
                case "ctrl":
                case "leftMeta":
                case "rightMeta":
                case "contextMenu":
                case "backspace":
                case "pageDown":
                case "pageUp":
                case "home":
                case "end":
                case "insert":
                case "delete":
                case "capsLock":
                case "numLock":
                case "printScreen":
                case "scrollLock":
                case "pause":
                case "numpadEnter":
                case "numpadDivide":
                case "numpadMultiply":
                case "numpadPlus":
                case "numpadMinus":
                case "numpadPeriod":
                case "numpadEquals":
                case "numpad1":
                case "numpad2":
                case "numpad3":
                case "numpad4":
                case "numpad5":
                case "numpad6":
                case "numpad7":
                case "numpad8":
                case "numpad9":
                case "numpad0":
                case "f1":
                case "f2":
                case "f3":
                case "f4":
                case "f5":
                case "f6":
                case "f7":
                case "f8":
                case "f9":
                case "f10":
                case "f11":
                case "f12":
                case "wasd":
                case "arrowkeys":
                    return true;
                default:
                    return false;
            }
        }

        public override IEnumerable<ControlButtonMetadata> GetAll()
        {
            if (_controls != null)
            {
                return _controls;
            }

            _controls = new[]
            {
                escape,
                space,
                enter,
                tab,
                backquote,
                quote,
                semicolon,
                comma,
                period,
                slash,
                backslash,
                leftBracket,
                rightBracket,
                minus,
                equals,
                upArrow,
                downArrow,
                leftArrow,
                rightArrow,
                a,
                b,
                c,
                d,
                e,
                f,
                g,
                h,
                i,
                j,
                k,
                l,
                m,
                n,
                o,
                p,
                q,
                r,
                s,
                t,
                u,
                v,
                w,
                x,
                y,
                z,
                _1,
                _2,
                _3,
                _4,
                _5,
                _6,
                _7,
                _8,
                _9,
                _0,
                leftShift,
                rightShift,
                shift,
                leftAlt,
                rightAlt,
                alt,
                leftCtrl,
                rightCtrl,
                ctrl,
                leftMeta,
                rightMeta,
                contextMenu,
                backspace,
                pageDown,
                pageUp,
                home,
                end,
                insert,
                delete,
                capsLock,
                numLock,
                printScreen,
                scrollLock,
                pause,
                numpadEnter,
                numpadDivide,
                numpadMultiply,
                numpadPlus,
                numpadMinus,
                numpadPeriod,
                numpadEquals,
                numpad1,
                numpad2,
                numpad3,
                numpad4,
                numpad5,
                numpad6,
                numpad7,
                numpad8,
                numpad9,
                numpad0,
                f1,
                f2,
                f3,
                f4,
                f5,
                f6,
                f7,
                f8,
                f9,
                f10,
                f11,
                f12,
                wasd,
                arrowkeys
            };

            return _controls;
        }

        public override ControlButtonMetadata Resolve(InputControl control)
        {
            switch (control.name)
            {
                case "escape":
                    return escape;
                case "space":
                    return space;
                case "enter":
                    return enter;
                case "tab":
                    return tab;
                case "backquote":
                    return backquote;
                case "quote":
                    return quote;
                case "semicolon":
                    return semicolon;
                case "comma":
                    return comma;
                case "period":
                    return period;
                case "slash":
                    return slash;
                case "backslash":
                    return backslash;
                case "leftBracket":
                    return leftBracket;
                case "rightBracket":
                    return rightBracket;
                case "minus":
                    return minus;
                case "equals":
                    return equals;
                case "upArrow":
                    return upArrow;
                case "downArrow":
                    return downArrow;
                case "leftArrow":
                    return leftArrow;
                case "rightArrow":
                    return rightArrow;
                case "a":
                    return a;
                case "b":
                    return b;
                case "c":
                    return c;
                case "d":
                    return d;
                case "e":
                    return e;
                case "f":
                    return f;
                case "g":
                    return g;
                case "h":
                    return h;
                case "i":
                    return i;
                case "j":
                    return j;
                case "k":
                    return k;
                case "l":
                    return l;
                case "m":
                    return m;
                case "n":
                    return n;
                case "o":
                    return o;
                case "p":
                    return p;
                case "q":
                    return q;
                case "r":
                    return r;
                case "s":
                    return s;
                case "t":
                    return t;
                case "u":
                    return u;
                case "v":
                    return v;
                case "w":
                    return w;
                case "x":
                    return x;
                case "y":
                    return y;
                case "z":
                    return z;
                case "1":
                    return _1;
                case "2":
                    return _2;
                case "3":
                    return _3;
                case "4":
                    return _4;
                case "5":
                    return _5;
                case "6":
                    return _6;
                case "7":
                    return _7;
                case "8":
                    return _8;
                case "9":
                    return _9;
                case "0":
                    return _0;
                case "leftShift":
                    return leftShift;
                case "rightShift":
                    return rightShift;
                case "shift":
                    return shift;
                case "leftAlt":
                    return leftAlt;
                case "rightAlt":
                    return rightAlt;
                case "alt":
                    return alt;
                case "leftCtrl":
                    return leftCtrl;
                case "rightCtrl":
                    return rightCtrl;
                case "ctrl":
                    return ctrl;
                case "leftMeta":
                    return leftMeta;
                case "rightMeta":
                    return rightMeta;
                case "contextMenu":
                    return contextMenu;
                case "backspace":
                    return backspace;
                case "pageDown":
                    return pageDown;
                case "pageUp":
                    return pageUp;
                case "home":
                    return home;
                case "end":
                    return end;
                case "insert":
                    return insert;
                case "delete":
                    return delete;
                case "capsLock":
                    return capsLock;
                case "numLock":
                    return numLock;
                case "printScreen":
                    return printScreen;
                case "scrollLock":
                    return scrollLock;
                case "pause":
                    return pause;
                case "numpadEnter":
                    return numpadEnter;
                case "numpadDivide":
                    return numpadDivide;
                case "numpadMultiply":
                    return numpadMultiply;
                case "numpadPlus":
                    return numpadPlus;
                case "numpadMinus":
                    return numpadMinus;
                case "numpadPeriod":
                    return numpadPeriod;
                case "numpadEquals":
                    return numpadEquals;
                case "numpad1":
                    return numpad1;
                case "numpad2":
                    return numpad2;
                case "numpad3":
                    return numpad3;
                case "numpad4":
                    return numpad4;
                case "numpad5":
                    return numpad5;
                case "numpad6":
                    return numpad6;
                case "numpad7":
                    return numpad7;
                case "numpad8":
                    return numpad8;
                case "numpad9":
                    return numpad9;
                case "numpad0":
                    return numpad0;
                case "f1":
                    return f1;
                case "f2":
                    return f2;
                case "f3":
                    return f3;
                case "f4":
                    return f4;
                case "f5":
                    return f5;
                case "f6":
                    return f6;
                case "f7":
                    return f7;
                case "f8":
                    return f8;
                case "f9":
                    return f9;
                case "f10":
                    return f10;
                case "f11":
                    return f11;
                case "f12":
                    return f12;
                case "wasd":
                    return wasd;
                case "arrowkeys":
                    return arrowkeys;
                default:
                    throw new NotSupportedException(control.name);
            }
        }

        internal override void PopulateAll()
        {
            using (_PRF_SetAll.Auto())
            {
                _ = GetAll();

                if (escape == null)
                {
                    escape = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(escape)}");
                    this.MarkAsModified();
                    _controls[0] = escape;
                }

                if (space == null)
                {
                    space = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(space)}");
                    this.MarkAsModified();
                    _controls[1] = space;
                }

                if (enter == null)
                {
                    enter = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(enter)}");
                    this.MarkAsModified();
                    _controls[2] = enter;
                }

                if (tab == null)
                {
                    tab = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(tab)}");
                    this.MarkAsModified();
                    _controls[3] = tab;
                }

                if (backquote == null)
                {
                    backquote = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(backquote)}");
                    this.MarkAsModified();
                    _controls[4] = backquote;
                }

                if (quote == null)
                {
                    quote = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(quote)}");
                    this.MarkAsModified();
                    _controls[5] = quote;
                }

                if (semicolon == null)
                {
                    semicolon = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(semicolon)}");
                    this.MarkAsModified();
                    _controls[6] = semicolon;
                }

                if (comma == null)
                {
                    comma = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(comma)}");
                    this.MarkAsModified();
                    _controls[7] = comma;
                }

                if (period == null)
                {
                    period = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(period)}");
                    this.MarkAsModified();
                    _controls[8] = period;
                }

                if (slash == null)
                {
                    slash = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(slash)}");
                    this.MarkAsModified();
                    _controls[9] = slash;
                }

                if (backslash == null)
                {
                    backslash = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(backslash)}");
                    this.MarkAsModified();
                    _controls[10] = backslash;
                }

                if (leftBracket == null)
                {
                    leftBracket =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(leftBracket)}");
                    this.MarkAsModified();
                    _controls[11] = leftBracket;
                }

                if (rightBracket == null)
                {
                    rightBracket =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(rightBracket)}");
                    this.MarkAsModified();
                    _controls[12] = rightBracket;
                }

                if (minus == null)
                {
                    minus = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(minus)}");
                    this.MarkAsModified();
                    _controls[13] = minus;
                }

                if (equals == null)
                {
                    equals = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(equals)}");
                    this.MarkAsModified();
                    _controls[14] = equals;
                }

                if (upArrow == null)
                {
                    upArrow = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(upArrow)}");
                    this.MarkAsModified();
                    _controls[15] = upArrow;
                }

                if (downArrow == null)
                {
                    downArrow = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(downArrow)}");
                    this.MarkAsModified();
                    _controls[16] = downArrow;
                }

                if (leftArrow == null)
                {
                    leftArrow = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(leftArrow)}");
                    this.MarkAsModified();
                    _controls[17] = leftArrow;
                }

                if (rightArrow == null)
                {
                    rightArrow = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(rightArrow)}");
                    this.MarkAsModified();
                    _controls[18] = rightArrow;
                }

                if (a == null)
                {
                    a = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(a)}");
                    this.MarkAsModified();
                    _controls[19] = a;
                }

                if (b == null)
                {
                    b = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(b)}");
                    this.MarkAsModified();
                    _controls[20] = b;
                }

                if (c == null)
                {
                    c = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(c)}");
                    this.MarkAsModified();
                    _controls[21] = c;
                }

                if (d == null)
                {
                    d = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(d)}");
                    this.MarkAsModified();
                    _controls[22] = d;
                }

                if (e == null)
                {
                    e = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(e)}");
                    this.MarkAsModified();
                    _controls[23] = e;
                }

                if (f == null)
                {
                    f = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f)}");
                    this.MarkAsModified();
                    _controls[24] = f;
                }

                if (g == null)
                {
                    g = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(g)}");
                    this.MarkAsModified();
                    _controls[25] = g;
                }

                if (h == null)
                {
                    h = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(h)}");
                    this.MarkAsModified();
                    _controls[26] = h;
                }

                if (i == null)
                {
                    i = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(i)}");
                    this.MarkAsModified();
                    _controls[27] = i;
                }

                if (j == null)
                {
                    j = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(j)}");
                    this.MarkAsModified();
                    _controls[28] = j;
                }

                if (k == null)
                {
                    k = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(k)}");
                    this.MarkAsModified();
                    _controls[29] = k;
                }

                if (l == null)
                {
                    l = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(l)}");
                    this.MarkAsModified();
                    _controls[30] = l;
                }

                if (m == null)
                {
                    m = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(m)}");
                    this.MarkAsModified();
                    _controls[31] = m;
                }

                if (n == null)
                {
                    n = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(n)}");
                    this.MarkAsModified();
                    _controls[32] = n;
                }

                if (o == null)
                {
                    o = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(o)}");
                    this.MarkAsModified();
                    _controls[33] = o;
                }

                if (p == null)
                {
                    p = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(p)}");
                    this.MarkAsModified();
                    _controls[34] = p;
                }

                if (q == null)
                {
                    q = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(q)}");
                    this.MarkAsModified();
                    _controls[35] = q;
                }

                if (r == null)
                {
                    r = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(r)}");
                    this.MarkAsModified();
                    _controls[36] = r;
                }

                if (s == null)
                {
                    s = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(s)}");
                    this.MarkAsModified();
                    _controls[37] = s;
                }

                if (t == null)
                {
                    t = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(t)}");
                    this.MarkAsModified();
                    _controls[38] = t;
                }

                if (u == null)
                {
                    u = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(u)}");
                    this.MarkAsModified();
                    _controls[39] = u;
                }

                if (v == null)
                {
                    v = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(v)}");
                    this.MarkAsModified();
                    _controls[40] = v;
                }

                if (w == null)
                {
                    w = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(w)}");
                    this.MarkAsModified();
                    _controls[41] = w;
                }

                if (x == null)
                {
                    x = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(x)}");
                    this.MarkAsModified();
                    _controls[42] = x;
                }

                if (y == null)
                {
                    y = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(y)}");
                    this.MarkAsModified();
                    _controls[43] = y;
                }

                if (z == null)
                {
                    z = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(z)}");
                    this.MarkAsModified();
                    _controls[44] = z;
                }

                if (_1 == null)
                {
                    _1 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_1)}");
                    this.MarkAsModified();
                    _controls[45] = _1;
                }

                if (_2 == null)
                {
                    _2 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_2)}");
                    this.MarkAsModified();
                    _controls[46] = _2;
                }

                if (_3 == null)
                {
                    _3 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_3)}");
                    this.MarkAsModified();
                    _controls[47] = _3;
                }

                if (_4 == null)
                {
                    _4 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_4)}");
                    this.MarkAsModified();
                    _controls[48] = _4;
                }

                if (_5 == null)
                {
                    _5 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_5)}");
                    this.MarkAsModified();
                    _controls[49] = _5;
                }

                if (_6 == null)
                {
                    _6 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_6)}");
                    this.MarkAsModified();
                    _controls[50] = _6;
                }

                if (_7 == null)
                {
                    _7 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_7)}");
                    this.MarkAsModified();
                    _controls[51] = _7;
                }

                if (_8 == null)
                {
                    _8 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_8)}");
                    this.MarkAsModified();
                    _controls[52] = _8;
                }

                if (_9 == null)
                {
                    _9 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_9)}");
                    this.MarkAsModified();
                    _controls[53] = _9;
                }

                if (_0 == null)
                {
                    _0 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(_0)}");
                    this.MarkAsModified();
                    _controls[54] = _0;
                }

                if (leftShift == null)
                {
                    leftShift = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(leftShift)}");
                    this.MarkAsModified();
                    _controls[55] = leftShift;
                }

                if (rightShift == null)
                {
                    rightShift = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(rightShift)}");
                    this.MarkAsModified();
                    _controls[56] = rightShift;
                }

                if (shift == null)
                {
                    shift = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(shift)}");
                    this.MarkAsModified();
                    _controls[57] = shift;
                }

                if (leftAlt == null)
                {
                    leftAlt = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(leftAlt)}");
                    this.MarkAsModified();
                    _controls[58] = leftAlt;
                }

                if (rightAlt == null)
                {
                    rightAlt = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(rightAlt)}");
                    this.MarkAsModified();
                    _controls[59] = rightAlt;
                }

                if (alt == null)
                {
                    alt = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(alt)}");
                    this.MarkAsModified();
                    _controls[60] = alt;
                }

                if (leftCtrl == null)
                {
                    leftCtrl = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(leftCtrl)}");
                    this.MarkAsModified();
                    _controls[61] = leftCtrl;
                }

                if (rightCtrl == null)
                {
                    rightCtrl = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(rightCtrl)}");
                    this.MarkAsModified();
                    _controls[62] = rightCtrl;
                }

                if (ctrl == null)
                {
                    ctrl = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(ctrl)}");
                    this.MarkAsModified();
                    _controls[63] = ctrl;
                }

                if (leftMeta == null)
                {
                    leftMeta = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(leftMeta)}");
                    this.MarkAsModified();
                    _controls[64] = leftMeta;
                }

                if (rightMeta == null)
                {
                    rightMeta = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(rightMeta)}");
                    this.MarkAsModified();
                    _controls[65] = rightMeta;
                }

                if (contextMenu == null)
                {
                    contextMenu =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(contextMenu)}");
                    this.MarkAsModified();
                    _controls[66] = contextMenu;
                }

                if (backspace == null)
                {
                    backspace = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(backspace)}");
                    this.MarkAsModified();
                    _controls[67] = backspace;
                }

                if (pageDown == null)
                {
                    pageDown = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(pageDown)}");
                    this.MarkAsModified();
                    _controls[68] = pageDown;
                }

                if (pageUp == null)
                {
                    pageUp = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(pageUp)}");
                    this.MarkAsModified();
                    _controls[69] = pageUp;
                }

                if (home == null)
                {
                    home = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(home)}");
                    this.MarkAsModified();
                    _controls[70] = home;
                }

                if (end == null)
                {
                    end = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(end)}");
                    this.MarkAsModified();
                    _controls[71] = end;
                }

                if (insert == null)
                {
                    insert = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(insert)}");
                    this.MarkAsModified();
                    _controls[72] = insert;
                }

                if (delete == null)
                {
                    delete = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(delete)}");
                    this.MarkAsModified();
                    _controls[73] = delete;
                }

                if (capsLock == null)
                {
                    capsLock = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(capsLock)}");
                    this.MarkAsModified();
                    _controls[74] = capsLock;
                }

                if (numLock == null)
                {
                    numLock = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numLock)}");
                    this.MarkAsModified();
                    _controls[75] = numLock;
                }

                if (printScreen == null)
                {
                    printScreen =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(printScreen)}");
                    this.MarkAsModified();
                    _controls[76] = printScreen;
                }

                if (scrollLock == null)
                {
                    scrollLock = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(scrollLock)}");
                    this.MarkAsModified();
                    _controls[77] = scrollLock;
                }

                if (pause == null)
                {
                    pause = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(pause)}");
                    this.MarkAsModified();
                    _controls[78] = pause;
                }

                if (numpadEnter == null)
                {
                    numpadEnter =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpadEnter)}");
                    this.MarkAsModified();
                    _controls[79] = numpadEnter;
                }

                if (numpadDivide == null)
                {
                    numpadDivide =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpadDivide)}");
                    this.MarkAsModified();
                    _controls[80] = numpadDivide;
                }

                if (numpadMultiply == null)
                {
                    numpadMultiply =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpadMultiply)}");
                    this.MarkAsModified();
                    _controls[81] = numpadMultiply;
                }

                if (numpadPlus == null)
                {
                    numpadPlus = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpadPlus)}");
                    this.MarkAsModified();
                    _controls[82] = numpadPlus;
                }

                if (numpadMinus == null)
                {
                    numpadMinus =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpadMinus)}");
                    this.MarkAsModified();
                    _controls[83] = numpadMinus;
                }

                if (numpadPeriod == null)
                {
                    numpadPeriod =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpadPeriod)}");
                    this.MarkAsModified();
                    _controls[84] = numpadPeriod;
                }

                if (numpadEquals == null)
                {
                    numpadEquals =
                        LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpadEquals)}");
                    this.MarkAsModified();
                    _controls[85] = numpadEquals;
                }

                if (numpad1 == null)
                {
                    numpad1 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad1)}");
                    this.MarkAsModified();
                    _controls[86] = numpad1;
                }

                if (numpad2 == null)
                {
                    numpad2 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad2)}");
                    this.MarkAsModified();
                    _controls[87] = numpad2;
                }

                if (numpad3 == null)
                {
                    numpad3 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad3)}");
                    this.MarkAsModified();
                    _controls[88] = numpad3;
                }

                if (numpad4 == null)
                {
                    numpad4 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad4)}");
                    this.MarkAsModified();
                    _controls[89] = numpad4;
                }

                if (numpad5 == null)
                {
                    numpad5 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad5)}");
                    this.MarkAsModified();
                    _controls[90] = numpad5;
                }

                if (numpad6 == null)
                {
                    numpad6 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad6)}");
                    this.MarkAsModified();
                    _controls[91] = numpad6;
                }

                if (numpad7 == null)
                {
                    numpad7 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad7)}");
                    this.MarkAsModified();
                    _controls[92] = numpad7;
                }

                if (numpad8 == null)
                {
                    numpad8 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad8)}");
                    this.MarkAsModified();
                    _controls[93] = numpad8;
                }

                if (numpad9 == null)
                {
                    numpad9 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad9)}");
                    this.MarkAsModified();
                    _controls[94] = numpad9;
                }

                if (numpad0 == null)
                {
                    numpad0 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(numpad0)}");
                    this.MarkAsModified();
                    _controls[95] = numpad0;
                }

                if (f1 == null)
                {
                    f1 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f1)}");
                    this.MarkAsModified();
                    _controls[96] = f1;
                }

                if (f2 == null)
                {
                    f2 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f2)}");
                    this.MarkAsModified();
                    _controls[97] = f2;
                }

                if (f3 == null)
                {
                    f3 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f3)}");
                    this.MarkAsModified();
                    _controls[98] = f3;
                }

                if (f4 == null)
                {
                    f4 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f4)}");
                    this.MarkAsModified();
                    _controls[99] = f4;
                }

                if (f5 == null)
                {
                    f5 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f5)}");
                    this.MarkAsModified();
                    _controls[100] = f5;
                }

                if (f6 == null)
                {
                    f6 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f6)}");
                    this.MarkAsModified();
                    _controls[101] = f6;
                }

                if (f7 == null)
                {
                    f7 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f7)}");
                    this.MarkAsModified();
                    _controls[102] = f7;
                }

                if (f8 == null)
                {
                    f8 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f8)}");
                    this.MarkAsModified();
                    _controls[103] = f8;
                }

                if (f9 == null)
                {
                    f9 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f9)}");
                    this.MarkAsModified();
                    _controls[104] = f9;
                }

                if (f10 == null)
                {
                    f10 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f10)}");
                    this.MarkAsModified();
                    _controls[105] = f10;
                }

                if (f11 == null)
                {
                    f11 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f11)}");
                    this.MarkAsModified();
                    _controls[106] = f11;
                }

                if (f12 == null)
                {
                    f12 = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(f12)}");
                    this.MarkAsModified();
                    _controls[107] = f12;
                }

                if (wasd == null)
                {
                    wasd = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(wasd)}");
                    this.MarkAsModified();
                    _controls[108] = wasd;
                }

                if (arrowkeys == null)
                {
                    arrowkeys = LoadOrCreateNew<ControlButtonMetadata>($"{deviceName}/{nameof(arrowkeys)}");
                    this.MarkAsModified();
                    _controls[109] = arrowkeys;
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(KeyboardMetadata) + ".";

        private static readonly ProfilerMarker _PRF_SetAll =
            new ProfilerMarker(_PRF_PFX + nameof(PopulateAll));

        private static readonly ProfilerMarker _PRF_CanResolve =
            new ProfilerMarker(_PRF_PFX + nameof(CanResolve));

        private static readonly ProfilerMarker _PRF_Resolve = new ProfilerMarker(_PRF_PFX + nameof(Resolve));

        #endregion
    }
}
