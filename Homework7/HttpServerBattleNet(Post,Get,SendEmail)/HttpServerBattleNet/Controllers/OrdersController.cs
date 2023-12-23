using System.Text;
using HtmlAgilityPack;
using HttpServerBattleNet.Attribuets;
using HttpServerBattleNet.Model;

namespace HttpServerBattleNet.Controllers;

[Controller("Orders")]
public class OrdersController
{
    [Get("list")]
    public string List()
    {
        var html = @"https://steamcommunity.com/market/";
        var html2 = @"https://steamcommunity.com/market/search?q=#p2_popular_desc";
        var web = new HtmlWeb();
        var htmlDoc2 = web.Load(html2);
        var htmlDoc = web.Load(html);
        var parserSingle = htmlDoc.DocumentNode.SelectNodes("//a[contains(@class, 'market_listing_row')]");
        var parseSecondPage = htmlDoc2.DocumentNode.SelectNodes("//a[contains(@class, 'market_listing_row_link')]");

        var modelSite = new List<CardModel>();
        var sb = new StringBuilder();
        sb.Append($@"
                            <!doctype html>
                             <html lang=""en"">
                             <head>
                                 <meta charset=""UTF-8"">
                                 <meta name=""viewport""
                                       content=""width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0"">
                                 <meta http-equiv=""X-UA-Compatible"" content=""ie=edge"">
                                 <title>Document</title>
                                 <style>
                                    body{{
                                      display: flex;
                                      flex-wrap: wrap;
                                      max-width: 1200px
                                      padding: 50px;
                                      justify-content: center;  
                                    }}
                                    button {{
                                         border: none;
                                         background-color: aqua;
                                         padding: 10px;
                                         border-radius: 30px;
                                         width: 50px;
                                     }}
                                     
                                     .wrapper{{
                                         text-align: center;
                                         padding: 15px;
                                         width: 300px;
                                         border: 1px solid gray;
                                     }}
                
                                 </style>
                             </head>
                             <body>
                                 
                             </body>
                             </html>
                         ");

        foreach (var htmlNode in parserSingle)
        {
            var imageNode = htmlNode.SelectSingleNode(".//img[@class='market_listing_item_img']");
            var imageUrl = imageNode?.GetAttributeValue("src", "");

            var nameNode = htmlNode.SelectSingleNode(".//span[@class='market_listing_item_name']");
            var itemName = nameNode?.InnerText.Trim();

            var gameNode = htmlNode.SelectSingleNode(".//span[@class='market_listing_game_name']");
            var gameName = gameNode?.InnerText.Trim();

            var numListingsNode = htmlNode.SelectSingleNode(".//span[@class='market_listing_num_listings_qty']");
            var numListings = numListingsNode?.InnerText.Trim();

            var normalPriceNode = htmlNode.SelectSingleNode(".//span[@class='normal_price']");
            var normalPrice = normalPriceNode?.InnerText.Trim();

            var salePriceNode = htmlNode.SelectSingleNode(".//span[@class='sale_price']");
            var salePrice = salePriceNode?.InnerText.Trim();


            modelSite.Add(new CardModel()
            {
                Image = imageUrl, NamgGame = gameName, NormalPrice = normalPrice, SalePrice = salePrice,
                NameItem = itemName
            });
        }

        foreach (var item in parseSecondPage)
        {
            var imageNode = item.SelectSingleNode(".//img[@class='market_listing_item_img']");
            var imageUrl = imageNode?.GetAttributeValue("src", "");

            var nameNode = item.SelectSingleNode(".//span[@class='market_listing_item_name']");
            var itemName = nameNode?.InnerText.Trim();

            var gameNode = item.SelectSingleNode(".//span[@class='market_listing_game_name']");
            var gameName = gameNode?.InnerText.Trim();

            var numListingsNode = item.SelectSingleNode(".//span[@class='market_listing_num_listings_qty']");
            var numListings = numListingsNode?.InnerText.Trim();

            var normalPriceNode = item.SelectSingleNode(".//span[@class='normal_price']");
            var normalPrice = normalPriceNode?.InnerText.Trim();

            var salePriceNode = item.SelectSingleNode(".//span[@class='sale_price']");
            var salePrice = salePriceNode?.InnerText.Trim();

            modelSite.Add(new CardModel()
            {
                Image = imageUrl, NamgGame = gameName, NormalPrice = normalPrice, SalePrice = salePrice,
                NameItem = itemName
            });
        }

        foreach (var model in modelSite)
        {
            sb.AppendFormat($@"
                           <div class=""wrapper"">
                                  <img src=""{model.Image}"" alt="""">
                                  <p>{model.NamgGame}</p>
                                  <p>{model.NameItem}</p>
                                  <p>{model.NormalPrice}</p>
                                  <p>{model.SalePrice}</p>
                                  <button type=""""submit"""">Buy</button>
                            </div>");
        }

        return sb.ToString();
    }
}