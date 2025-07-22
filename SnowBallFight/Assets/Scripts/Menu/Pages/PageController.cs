using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    namespace Menu
    {
        public class PageController : MonoBehaviourSingleton<PageController>
        {
            public Page[] pages;

            private Hashtable m_Pages;

            public PageType activePageType = PageType.none;

            protected override void Awake()
            {
                base.Awake();
                m_Pages = new Hashtable();
                RegistryAllPage();
            }

            #region publicFunctions
            public void TurnPageOn(PageType _pageType)
            {
                if (_pageType == PageType.none) return;
                if (!PageExists(_pageType)) return;
                if (activePageType != PageType.none)
                {
                    Page _offPage = GetPage(activePageType);
                    _offPage.ClosePage();
                }

                Page _page = GetPage(_pageType);
                activePageType = _page.pageType;
                _page.OpenPage();

            }

            internal void TurnPageOff(PageType pageType)
            {
                if (activePageType != pageType) return;
                if (!PageExists(pageType)) return;

                Page page = GetPage(pageType);
                page.ClosePage();
            }

            public void ChangePage(PageType off, PageType on)
            {
                if (off == PageType.none) return;
                if (!PageExists(off)) return;

                Page page = GetPage(off);
                page.ClosePage();
                activePageType = PageType.none;

                TurnPageOn(on);
            }

            public bool StepBackPage()
            {
                if (activePageType == PageType.none) return false;
                if (!PageExists(activePageType)) return false;

                Page activePage = GetPage(activePageType);

                if (activePage.previousPageType == PageType.none) return false;

                ChangePage(activePage.pageType, activePage.previousPageType);

                return true;
            }

            #endregion

            #region privateFunctions
            private void RegistryAllPage()
            {
                foreach (Page _page in pages)
                {
                    RegisterPage(_page);
                }
            }

            private void RegisterPage(Page _page)
            {
                if (PageExists(_page.pageType)) return;


                m_Pages.Add(_page.pageType, _page);

            }

            private Page GetPage(PageType _pageType)
            {
                if (!PageExists(_pageType)) return null;


                return (Page)m_Pages[_pageType];
            }

            private bool PageExists(PageType _pageType)
            {
                return m_Pages.ContainsKey(_pageType);
            }
            #endregion
        }

    }
}

