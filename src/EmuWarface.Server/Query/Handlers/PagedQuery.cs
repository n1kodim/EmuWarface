using EmuWarface.Server.Common;
using EmuWarface.Server.Common.Attributes;
using EmuWarface.Server.Game.Data.Cache;
using EmuWarface.Server.Game.Player;
using MiniXML;
using NLog;

namespace EmuWarface.Server.Query.Handlers
{
    #region Xml samples

    /*
    Request:   masterserver@warface/xxx
    <items/>
    <items from='250' hash='1487642888'/>


    Response:   ms.warface

    2 - HasMore
    <data query_name='items' code='2' from='250' to='500' hash='1487642888' compressedData='xxx' originalSize='xxx'/>
    3 - Done
    <data query_name='items' code='3' from='4250' to='4383' hash='1487642888' compressedData='xxx' originalSize='xxx'/>
    */

    #endregion

    [Query("items")]
    public class PagedQuery : QueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private CacheStoreManager _cacheSystem;

        public PagedQuery(CacheStoreManager cacheSystem)
        {
            _cacheSystem = cacheSystem;
        }

        public override async Task<Result<Element?, int>> HandleRequestAsync(IOnlinePlayer player, Element query)
        {
            // TODO: test if from="asd"
            var from = query.GetAttributeValue<int>("from");
			//var to = query.Element.HasAttribute("to") ? query.Element.GetAttributeInt("to") : int.MaxValue;
			var requestHash = query.GetAttribute("hash");
			var cachedHash = query.GetAttribute("cached");

            var data = _cacheSystem.GetCache(query.LocalName);
            if (data == null)
            {
                _logger.Error("Not found cache with name '{0}'", query.LocalName);
                return -1;
            }

            Element res = new Element(query.Name)
                .Attr("code", (int)CacheCode.NotModified)
                .Attr("from", 0)
                .Attr("to", 0)
                .Attr("hash", data.Hash);

            if (cachedHash == data.Hash)
                return res;

            if (from < 0 /*|| to <= from*/)
                return -1;

            if(!string.IsNullOrEmpty(requestHash) && requestHash != data.Hash)
            {
                res.Attr("code", (int)CacheCode.RequestSequenceInterrupted);
                return res;
            }

            var index = from / 250;
            var splitted = data.GetSplittedElement(index);
            if (splitted == null)
            {
                _logger.Warn("'{0}': index({1}) of splitted items exceeded the limits.", query.LocalName, index);
                return -1;
            }

            res = splitted;
            res.Name = query.Name;

            return res;
        }


    }
}
