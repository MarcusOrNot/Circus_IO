mergeInto(LibraryManager.library, {

  Purchase: function(id) {
    buy(UTF8ToString(id));
  },
  
  GetCatalog: function() {
    getCatalog();
  },
  
  GetPurchases: function() {
	getPurchases();
  },

  AuthenticateUser: function() {
    auth();
  },

  GetUserData: function() {
    getUserData();
  },

  ShowFullscreenAd: function () {
    showFullscrenAd();
  },

  ShowRewardedAd: function(placement) {
    showRewardedAd(placement);
    return placement;
  },

  OpenWindow: function(link){
    var url = Pointer_stringify(link);
      document.onmouseup = function()
      {
        window.open(url);
        document.onmouseup = null;
      }
  },
  
  SetGameData: function(gamedataString) {
	setGameData(UTF8ToString(gamedataString));
  },
  
  GetYandexGameData: function(keyJSON) {
	getGameData(UTF8ToString(keyJSON));
  },
  
  SetGameStat: function(statdataString) {
	setGameStat(UTF8ToString(statdataString));
  },
  
  GetYandexGameStat: function(keyJSON) {
	getGameStat(UTF8ToString(keyJSON));
  },
  
  GetYandexLeaderboard: function(leaderboardName, quantityTop, includeUser, quantityAround) {
	getLeaderboard(UTF8ToString(leaderboardName), quantityTop, includeUser, quantityAround);
  },
  
  SetYandexLeaderboardScore: function(leaderboardName, score) {
	setLeaderboardScore(UTF8ToString(leaderboardName), score);
  },
  
  GetLanguage: function() {
	getLanguage();
  },
  
});