var _defaultRoot = null

var vueUrl = {
  install: function (Vue, options) {
    if (typeof options === 'string') {
        _defaultRoot = options
    }
    Vue.getUrl = getUrl;

    Vue.prototype.$getUrl = getUrl;
  }
}

function getUrl (url) {
   return _defaultRoot+url;
}


module.exports= vueUrl;