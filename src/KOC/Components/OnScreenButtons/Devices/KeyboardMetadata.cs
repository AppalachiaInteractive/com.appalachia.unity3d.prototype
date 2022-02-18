using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Components.OnScreenButtons.Controls;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons.Devices
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

#if UNITY_EDITOR
        /// <inheritdoc />
        internal override void PopulateAll()
        {
            using (_PRF_SetAll.Auto())
            {
                _ = GetAll();

                if (escape == null)
                {
                    escape = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(escape))
                    );
                    MarkAsModified();
                    _controls[0] = escape;
                }

                if (space == null)
                {
                    space = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(space))
                    );
                    MarkAsModified();
                    _controls[1] = space;
                }

                if (enter == null)
                {
                    enter = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(enter))
                    );
                    MarkAsModified();
                    _controls[2] = enter;
                }

                if (tab == null)
                {
                    tab = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(tab))
                    );
                    MarkAsModified();
                    _controls[3] = tab;
                }

                if (backquote == null)
                {
                    backquote = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(backquote))
                    );
                    MarkAsModified();
                    _controls[4] = backquote;
                }

                if (quote == null)
                {
                    quote = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(quote))
                    );
                    MarkAsModified();
                    _controls[5] = quote;
                }

                if (semicolon == null)
                {
                    semicolon = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(semicolon))
                    );
                    MarkAsModified();
                    _controls[6] = semicolon;
                }

                if (comma == null)
                {
                    comma = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(comma))
                    );
                    MarkAsModified();
                    _controls[7] = comma;
                }

                if (period == null)
                {
                    period = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(period))
                    );
                    MarkAsModified();
                    _controls[8] = period;
                }

                if (slash == null)
                {
                    slash = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(slash))
                    );
                    MarkAsModified();
                    _controls[9] = slash;
                }

                if (backslash == null)
                {
                    backslash = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(backslash))
                    );
                    MarkAsModified();
                    _controls[10] = backslash;
                }

                if (leftBracket == null)
                {
                    leftBracket = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftBracket))
                    );
                    MarkAsModified();
                    _controls[11] = leftBracket;
                }

                if (rightBracket == null)
                {
                    rightBracket = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightBracket))
                    );
                    MarkAsModified();
                    _controls[12] = rightBracket;
                }

                if (minus == null)
                {
                    minus = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(minus))
                    );
                    MarkAsModified();
                    _controls[13] = minus;
                }

                if (equals == null)
                {
                    equals = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(equals))
                    );
                    MarkAsModified();
                    _controls[14] = equals;
                }

                if (upArrow == null)
                {
                    upArrow = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(upArrow))
                    );
                    MarkAsModified();
                    _controls[15] = upArrow;
                }

                if (downArrow == null)
                {
                    downArrow = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(downArrow))
                    );
                    MarkAsModified();
                    _controls[16] = downArrow;
                }

                if (leftArrow == null)
                {
                    leftArrow = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftArrow))
                    );
                    MarkAsModified();
                    _controls[17] = leftArrow;
                }

                if (rightArrow == null)
                {
                    rightArrow = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightArrow))
                    );
                    MarkAsModified();
                    _controls[18] = rightArrow;
                }

                if (a == null)
                {
                    a = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(a))
                    );
                    MarkAsModified();
                    _controls[19] = a;
                }

                if (b == null)
                {
                    b = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(b))
                    );
                    MarkAsModified();
                    _controls[20] = b;
                }

                if (c == null)
                {
                    c = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(c))
                    );
                    MarkAsModified();
                    _controls[21] = c;
                }

                if (d == null)
                {
                    d = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(d))
                    );
                    MarkAsModified();
                    _controls[22] = d;
                }

                if (e == null)
                {
                    e = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(e))
                    );
                    MarkAsModified();
                    _controls[23] = e;
                }

                if (f == null)
                {
                    f = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f))
                    );
                    MarkAsModified();
                    _controls[24] = f;
                }

                if (g == null)
                {
                    g = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(g))
                    );
                    MarkAsModified();
                    _controls[25] = g;
                }

                if (h == null)
                {
                    h = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(h))
                    );
                    MarkAsModified();
                    _controls[26] = h;
                }

                if (i == null)
                {
                    i = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(i))
                    );
                    MarkAsModified();
                    _controls[27] = i;
                }

                if (j == null)
                {
                    j = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(j))
                    );
                    MarkAsModified();
                    _controls[28] = j;
                }

                if (k == null)
                {
                    k = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(k))
                    );
                    MarkAsModified();
                    _controls[29] = k;
                }

                if (l == null)
                {
                    l = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(l))
                    );
                    MarkAsModified();
                    _controls[30] = l;
                }

                if (m == null)
                {
                    m = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(m))
                    );
                    MarkAsModified();
                    _controls[31] = m;
                }

                if (n == null)
                {
                    n = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(n))
                    );
                    MarkAsModified();
                    _controls[32] = n;
                }

                if (o == null)
                {
                    o = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(o))
                    );
                    MarkAsModified();
                    _controls[33] = o;
                }

                if (p == null)
                {
                    p = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(p))
                    );
                    MarkAsModified();
                    _controls[34] = p;
                }

                if (q == null)
                {
                    q = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(q))
                    );
                    MarkAsModified();
                    _controls[35] = q;
                }

                if (r == null)
                {
                    r = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(r))
                    );
                    MarkAsModified();
                    _controls[36] = r;
                }

                if (s == null)
                {
                    s = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(s))
                    );
                    MarkAsModified();
                    _controls[37] = s;
                }

                if (t == null)
                {
                    t = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(t))
                    );
                    MarkAsModified();
                    _controls[38] = t;
                }

                if (u == null)
                {
                    u = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(u))
                    );
                    MarkAsModified();
                    _controls[39] = u;
                }

                if (v == null)
                {
                    v = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(v))
                    );
                    MarkAsModified();
                    _controls[40] = v;
                }

                if (w == null)
                {
                    w = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(w))
                    );
                    MarkAsModified();
                    _controls[41] = w;
                }

                if (x == null)
                {
                    x = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(x))
                    );
                    MarkAsModified();
                    _controls[42] = x;
                }

                if (y == null)
                {
                    y = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(y))
                    );
                    MarkAsModified();
                    _controls[43] = y;
                }

                if (z == null)
                {
                    z = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(z))
                    );
                    MarkAsModified();
                    _controls[44] = z;
                }

                if (_1 == null)
                {
                    _1 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_1))
                    );
                    MarkAsModified();
                    _controls[45] = _1;
                }

                if (_2 == null)
                {
                    _2 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_2))
                    );
                    MarkAsModified();
                    _controls[46] = _2;
                }

                if (_3 == null)
                {
                    _3 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_3))
                    );
                    MarkAsModified();
                    _controls[47] = _3;
                }

                if (_4 == null)
                {
                    _4 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_4))
                    );
                    MarkAsModified();
                    _controls[48] = _4;
                }

                if (_5 == null)
                {
                    _5 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_5))
                    );
                    MarkAsModified();
                    _controls[49] = _5;
                }

                if (_6 == null)
                {
                    _6 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_6))
                    );
                    MarkAsModified();
                    _controls[50] = _6;
                }

                if (_7 == null)
                {
                    _7 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_7))
                    );
                    MarkAsModified();
                    _controls[51] = _7;
                }

                if (_8 == null)
                {
                    _8 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_8))
                    );
                    MarkAsModified();
                    _controls[52] = _8;
                }

                if (_9 == null)
                {
                    _9 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_9))
                    );
                    MarkAsModified();
                    _controls[53] = _9;
                }

                if (_0 == null)
                {
                    _0 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(_0))
                    );
                    MarkAsModified();
                    _controls[54] = _0;
                }

                if (leftShift == null)
                {
                    leftShift = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftShift))
                    );
                    MarkAsModified();
                    _controls[55] = leftShift;
                }

                if (rightShift == null)
                {
                    rightShift = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightShift))
                    );
                    MarkAsModified();
                    _controls[56] = rightShift;
                }

                if (shift == null)
                {
                    shift = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(shift))
                    );
                    MarkAsModified();
                    _controls[57] = shift;
                }

                if (leftAlt == null)
                {
                    leftAlt = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftAlt))
                    );
                    MarkAsModified();
                    _controls[58] = leftAlt;
                }

                if (rightAlt == null)
                {
                    rightAlt = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightAlt))
                    );
                    MarkAsModified();
                    _controls[59] = rightAlt;
                }

                if (alt == null)
                {
                    alt = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(alt))
                    );
                    MarkAsModified();
                    _controls[60] = alt;
                }

                if (leftCtrl == null)
                {
                    leftCtrl = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftCtrl))
                    );
                    MarkAsModified();
                    _controls[61] = leftCtrl;
                }

                if (rightCtrl == null)
                {
                    rightCtrl = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightCtrl))
                    );
                    MarkAsModified();
                    _controls[62] = rightCtrl;
                }

                if (ctrl == null)
                {
                    ctrl = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(ctrl))
                    );
                    MarkAsModified();
                    _controls[63] = ctrl;
                }

                if (leftMeta == null)
                {
                    leftMeta = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftMeta))
                    );
                    MarkAsModified();
                    _controls[64] = leftMeta;
                }

                if (rightMeta == null)
                {
                    rightMeta = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightMeta))
                    );
                    MarkAsModified();
                    _controls[65] = rightMeta;
                }

                if (contextMenu == null)
                {
                    contextMenu = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(contextMenu))
                    );
                    MarkAsModified();
                    _controls[66] = contextMenu;
                }

                if (backspace == null)
                {
                    backspace = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(backspace))
                    );
                    MarkAsModified();
                    _controls[67] = backspace;
                }

                if (pageDown == null)
                {
                    pageDown = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(pageDown))
                    );
                    MarkAsModified();
                    _controls[68] = pageDown;
                }

                if (pageUp == null)
                {
                    pageUp = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(pageUp))
                    );
                    MarkAsModified();
                    _controls[69] = pageUp;
                }

                if (home == null)
                {
                    home = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(home))
                    );
                    MarkAsModified();
                    _controls[70] = home;
                }

                if (end == null)
                {
                    end = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(end))
                    );
                    MarkAsModified();
                    _controls[71] = end;
                }

                if (insert == null)
                {
                    insert = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(insert))
                    );
                    MarkAsModified();
                    _controls[72] = insert;
                }

                if (delete == null)
                {
                    delete = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(delete))
                    );
                    MarkAsModified();
                    _controls[73] = delete;
                }

                if (capsLock == null)
                {
                    capsLock = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(capsLock))
                    );
                    MarkAsModified();
                    _controls[74] = capsLock;
                }

                if (numLock == null)
                {
                    numLock = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numLock))
                    );
                    MarkAsModified();
                    _controls[75] = numLock;
                }

                if (printScreen == null)
                {
                    printScreen = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(printScreen))
                    );
                    MarkAsModified();
                    _controls[76] = printScreen;
                }

                if (scrollLock == null)
                {
                    scrollLock = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(scrollLock))
                    );
                    MarkAsModified();
                    _controls[77] = scrollLock;
                }

                if (pause == null)
                {
                    pause = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(pause))
                    );
                    MarkAsModified();
                    _controls[78] = pause;
                }

                if (numpadEnter == null)
                {
                    numpadEnter = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpadEnter))
                    );
                    MarkAsModified();
                    _controls[79] = numpadEnter;
                }

                if (numpadDivide == null)
                {
                    numpadDivide = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpadDivide))
                    );
                    MarkAsModified();
                    _controls[80] = numpadDivide;
                }

                if (numpadMultiply == null)
                {
                    numpadMultiply = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpadMultiply))
                    );
                    MarkAsModified();
                    _controls[81] = numpadMultiply;
                }

                if (numpadPlus == null)
                {
                    numpadPlus = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpadPlus))
                    );
                    MarkAsModified();
                    _controls[82] = numpadPlus;
                }

                if (numpadMinus == null)
                {
                    numpadMinus = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpadMinus))
                    );
                    MarkAsModified();
                    _controls[83] = numpadMinus;
                }

                if (numpadPeriod == null)
                {
                    numpadPeriod = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpadPeriod))
                    );
                    MarkAsModified();
                    _controls[84] = numpadPeriod;
                }

                if (numpadEquals == null)
                {
                    numpadEquals = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpadEquals))
                    );
                    MarkAsModified();
                    _controls[85] = numpadEquals;
                }

                if (numpad1 == null)
                {
                    numpad1 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad1))
                    );
                    MarkAsModified();
                    _controls[86] = numpad1;
                }

                if (numpad2 == null)
                {
                    numpad2 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad2))
                    );
                    MarkAsModified();
                    _controls[87] = numpad2;
                }

                if (numpad3 == null)
                {
                    numpad3 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad3))
                    );
                    MarkAsModified();
                    _controls[88] = numpad3;
                }

                if (numpad4 == null)
                {
                    numpad4 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad4))
                    );
                    MarkAsModified();
                    _controls[89] = numpad4;
                }

                if (numpad5 == null)
                {
                    numpad5 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad5))
                    );
                    MarkAsModified();
                    _controls[90] = numpad5;
                }

                if (numpad6 == null)
                {
                    numpad6 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad6))
                    );
                    MarkAsModified();
                    _controls[91] = numpad6;
                }

                if (numpad7 == null)
                {
                    numpad7 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad7))
                    );
                    MarkAsModified();
                    _controls[92] = numpad7;
                }

                if (numpad8 == null)
                {
                    numpad8 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad8))
                    );
                    MarkAsModified();
                    _controls[93] = numpad8;
                }

                if (numpad9 == null)
                {
                    numpad9 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad9))
                    );
                    MarkAsModified();
                    _controls[94] = numpad9;
                }

                if (numpad0 == null)
                {
                    numpad0 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(numpad0))
                    );
                    MarkAsModified();
                    _controls[95] = numpad0;
                }

                if (f1 == null)
                {
                    f1 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f1))
                    );
                    MarkAsModified();
                    _controls[96] = f1;
                }

                if (f2 == null)
                {
                    f2 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f2))
                    );
                    MarkAsModified();
                    _controls[97] = f2;
                }

                if (f3 == null)
                {
                    f3 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f3))
                    );
                    MarkAsModified();
                    _controls[98] = f3;
                }

                if (f4 == null)
                {
                    f4 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f4))
                    );
                    MarkAsModified();
                    _controls[99] = f4;
                }

                if (f5 == null)
                {
                    f5 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f5))
                    );
                    MarkAsModified();
                    _controls[100] = f5;
                }

                if (f6 == null)
                {
                    f6 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f6))
                    );
                    MarkAsModified();
                    _controls[101] = f6;
                }

                if (f7 == null)
                {
                    f7 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f7))
                    );
                    MarkAsModified();
                    _controls[102] = f7;
                }

                if (f8 == null)
                {
                    f8 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f8))
                    );
                    MarkAsModified();
                    _controls[103] = f8;
                }

                if (f9 == null)
                {
                    f9 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f9))
                    );
                    MarkAsModified();
                    _controls[104] = f9;
                }

                if (f10 == null)
                {
                    f10 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f10))
                    );
                    MarkAsModified();
                    _controls[105] = f10;
                }

                if (f11 == null)
                {
                    f11 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f11))
                    );
                    MarkAsModified();
                    _controls[106] = f11;
                }

                if (f12 == null)
                {
                    f12 = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(f12))
                    );
                    MarkAsModified();
                    _controls[107] = f12;
                }

                if (wasd == null)
                {
                    wasd = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(wasd))
                    );
                    MarkAsModified();
                    _controls[108] = wasd;
                }

                if (arrowkeys == null)
                {
                    arrowkeys = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(arrowkeys))
                    );
                    MarkAsModified();
                    _controls[109] = arrowkeys;
                }
            }
        }
#endif

        #region Profiling

        private const string _PRF_PFX = nameof(KeyboardMetadata) + ".";

        private static readonly ProfilerMarker _PRF_SetAll =
            new ProfilerMarker(_PRF_PFX + nameof(PopulateAll));

        private static readonly ProfilerMarker _PRF_CanResolve =
            new ProfilerMarker(_PRF_PFX + nameof(CanResolve));

        private static readonly ProfilerMarker _PRF_Resolve = new ProfilerMarker(_PRF_PFX + nameof(Resolve));

        #endregion

#if UNITY_EDITOR

#endif
    }
}
