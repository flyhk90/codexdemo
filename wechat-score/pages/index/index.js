import { api } from '../../utils/api.js';

Page({
  data: {
    score: 0,
    history: [],
    showHistory: false,
    scoreAnimation: '',
    loading: false
  },

  onLoad() {
    this.loadScore();
  },

  async loadScore() {
    this.setData({ loading: true });
    try {
      const response = await api.getScore();
      this.setData({
        score: response.score,
        history: response.history || []
      });
    } catch (error) {
      console.error('Failed to load score:', error);
    } finally {
      this.setData({ loading: false });
    }
  },

  async addScore(e) {
    const value = parseInt(e.currentTarget.dataset.value);
    this.setData({ scoreAnimation: 'scale' });
    
    try {
      const response = await api.addScore(value);
      setTimeout(() => {
        this.setData({
          score: response.score,
          history: response.history || [],
          scoreAnimation: ''
        });
      }, 150);
    } catch (error) {
      console.error('Failed to add score:', error);
      this.setData({ scoreAnimation: '' });
    }
  },

  async subtractScore(e) {
    const value = parseInt(e.currentTarget.dataset.value);
    this.setData({ scoreAnimation: 'scale' });
    
    try {
      const response = await api.subtractScore(value);
      setTimeout(() => {
        this.setData({
          score: response.score,
          history: response.history || [],
          scoreAnimation: ''
        });
      }, 150);
    } catch (error) {
      console.error('Failed to subtract score:', error);
      this.setData({ scoreAnimation: '' });
    }
  },

  async resetScore() {
    wx.showModal({
      title: '确认重置',
      content: '确定要将分数重置为0吗？',
      success: async (res) => {
        if (res.confirm) {
          try {
            const response = await api.resetScore();
            this.setData({
              score: response.score,
              history: response.history || []
            });
          } catch (error) {
            console.error('Failed to reset score:', error);
          }
        }
      }
    });
  },

  toggleHistory() {
    this.setData({ showHistory: !this.data.showHistory });
  },

  async clearHistory() {
    wx.showModal({
      title: '确认清空',
      content: '确定要清空所有历史记录吗？',
      success: async (res) => {
        if (res.confirm) {
          try {
            const response = await api.clearHistory();
            this.setData({
              history: response.history || []
            });
          } catch (error) {
            console.error('Failed to clear history:', error);
          }
        }
      }
    });
  },

  formatTime(dateString) {
    if (!dateString) return '';
    const date = new Date(dateString);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');
    return `${hours}:${minutes}:${seconds}`;
  }
});