<template>
  <section class="card">
    <div class="card-header">
      <div>
        <h2>接口状态</h2>
        <p>前端通过 `/api/health` 访问后端。</p>
      </div>
      <button class="action-button" :disabled="loading" @click="$emit('refresh')">
        {{ loading ? "检测中..." : "重新检测" }}
      </button>
    </div>

    <div class="status-grid">
      <article class="status-item">
        <span class="label">状态</span>
        <strong>{{ status }}</strong>
      </article>
      <article class="status-item">
        <span class="label">消息</span>
        <strong>{{ message }}</strong>
      </article>
      <article class="status-item">
        <span class="label">服务端时间</span>
        <strong>{{ checkedAt || "尚未返回" }}</strong>
      </article>
    </div>

    <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>
  </section>
</template>

<script setup>
defineProps({
  checkedAt: {
    type: String,
    default: ""
  },
  errorMessage: {
    type: String,
    default: ""
  },
  loading: {
    type: Boolean,
    default: false
  },
  message: {
    type: String,
    default: ""
  },
  status: {
    type: String,
    default: ""
  }
});

defineEmits(["refresh"]);
</script>
