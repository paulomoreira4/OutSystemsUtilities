using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

public class ESpaceList
{
    private const string TabTitle = "eSpaces";
    private RemoteWebDriver driver;
    public ESpaceList(RemoteWebDriver driver)
    {
        this.driver = driver;
        // トップメニューのアクティブなタブ内テキスト
        var activeTabTextInTheTopMenu = driver.FindElementByCssSelector(".Menu_TopMenuActive>a").Text;
        if (activeTabTextInTheTopMenu != ESpaceList.TabTitle)
        {
            throw new IllegalPageStateException(
                "選択されているタブが想定と異なります（想定：" + ESpaceList.TabTitle + "、実際：" + activeTabTextInTheTopMenu + "）");
        }
    }

    public ESpaceDesignFeedBack OpenESpace(string eSpaceName)
    {
        // 検索キーワードの設定
        var searchInput = this.driver.FindElementByCssSelector(".Filters input[type=text]");
        searchInput.Clear();
        searchInput.SendKeys(eSpaceName);
        // 検索ボタンクリック
        this.driver.FindElementByCssSelector(".Filters input[type=submit]").Click();
        // 検索結果の1ページ目にリンクテキストが、指定eSpace名であるLinkが表示される（＝検索される）のを待ってクリック
        var waitForLinkWitheSpaceName = new WebDriverWait(this.driver, new System.TimeSpan(0, 2, 0));
        // 匿名関数で実装すると、探索のたびに例外を投げるので、deprecatedだが、いったんExpectedConditionsで実装しておく
        waitForLinkWitheSpaceName.Until(ExpectedConditions.ElementIsVisible(By.LinkText(eSpaceName)))
                                 .Click();
        // 対象モジュールのドキュメント生成ページが開く
        return new ESpaceDesignFeedBack(this.driver, eSpaceName);
    }
}