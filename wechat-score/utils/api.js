const BASE_URL = 'http://localhost:5075';

export const api = {
  getScore: () => {
    return new Promise((resolve, reject) => {
      wx.request({
        url: `${BASE_URL}/api/score`,
        method: 'GET',
        success: (res) => resolve(res.data),
        fail: (err) => reject(err)
      });
    });
  },

  addScore: (value) => {
    return new Promise((resolve, reject) => {
      wx.request({
        url: `${BASE_URL}/api/score/add`,
        method: 'POST',
        data: { value },
        success: (res) => resolve(res.data),
        fail: (err) => reject(err)
      });
    });
  },

  subtractScore: (value) => {
    return new Promise((resolve, reject) => {
      wx.request({
        url: `${BASE_URL}/api/score/subtract`,
        method: 'POST',
        data: { value },
        success: (res) => resolve(res.data),
        fail: (err) => reject(err)
      });
    });
  },

  resetScore: () => {
    return new Promise((resolve, reject) => {
      wx.request({
        url: `${BASE_URL}/api/score/reset`,
        method: 'POST',
        success: (res) => resolve(res.data),
        fail: (err) => reject(err)
      });
    });
  },

  clearHistory: () => {
    return new Promise((resolve, reject) => {
      wx.request({
        url: `${BASE_URL}/api/score/history`,
        method: 'DELETE',
        success: (res) => resolve(res.data),
        fail: (err) => reject(err)
      });
    });
  }
};