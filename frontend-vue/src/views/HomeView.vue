<template>
  <main class="page">
    <section class="hero">
      <p class="eyebrow">Vue 3 + ASP.NET Core</p>
      <h1>前后端双项目样例</h1>
      <p class="lead">
        这个页面演示了一个常见的分离式结构：前端 Vue，后端 C# Web API。
      </p>
    </section>

    <StatusCard
      :status="status"
      :loading="loading"
      :error-message="errorMessage"
      :message="message"
      :checked-at="checkedAt"
      @refresh="loadHealth"
    />
  </main>
</template>

<script setup>
import { onMounted, ref } from "vue";
import StatusCard from "../components/StatusCard.vue";
import { getHealth } from "../services/api";

const status = ref("等待检测");
const message = ref("点击按钮测试前后端接口联通。");
const checkedAt = ref("");
const errorMessage = ref("");
const loading = ref(false);

async function loadHealth() {
  loading.value = true;
  errorMessage.value = "";

  try {
    const data = await getHealth();
    status.value = data.status;
    message.value = data.message;
    checkedAt.value = data.serverTime;
  } catch (error) {
    status.value = "请求失败";
    message.value = "前端已运行，但还没有成功连接到后端接口。";
    errorMessage.value = error.message;
    checkedAt.value = "";
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  loadHealth();
});
</script>
