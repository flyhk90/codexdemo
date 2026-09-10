# 首先从 transformers 库中导入 AutoTokenizer 类，它能自动适配不同大模型的分词规则
from transformers import AutoTokenizer
 
# 接着从预训练权重加载 Qwen2 模型的分词器
# 在线加载：下载到 D:\hf_cache\hub（也由环境变量 HF_HOME 全局控制）
tokenizer = AutoTokenizer.from_pretrained("Qwen/Qwen2-7B", cache_dir="D:/hf_cache/hub")
 
# 定义待处理的输入文本
text = "你好，我是cool。"
 
# ---------------------------------------------------------
# 第1步：分词 (Tokenization)
# ---------------------------------------------------------
# 使用 BPE算法将文本切分为“子词单元”
# 规则是：常见词为1个Token，复杂词会拆开，标点也算Token。
bpe_codes = tokenizer.tokenize(text)
# 先打印出来看一下结果
print(bpe_codes)
 
# 为了让分词结果可读，需要做一下处理
decoded_result = []
for bpe_code in bpe_codes:
    # 先将子词转换为模型词汇表中的 ID
    id = tokenizer.convert_tokens_to_ids(bpe_code)
    # 再将单个ID解码回文本并将结果存起来
    decoded = tokenizer.decode([id])
    decoded_result.append(decoded)
 
# 输出最终的分词列表
print("分词结果：", decoded_result)
 
# ---------------------------------------------------------
# 第2步：向量化 (Numericalization)
# ---------------------------------------------------------
# 将字符串形式的 Token 列表转换为模型能处理的整数 ID 列表
# 这是大模型的“输入语言”（模型只认识数字，不认识文字）
token_ids = tokenizer.convert_tokens_to_ids(bpe_codes)
print("向量ID：", token_ids)
# 把每个 ID 单独 decode 回文本，与第18行的 BPE 内部表示（"乱码"）形成对比
print("可读分词：", [tokenizer.decode([i]) for i in token_ids])
 
# ---------------------------------------------------------
# 第3步：统计 Token 数量
# ---------------------------------------------------------
# 计算 Token 总数
count = len(token_ids)
print("Token总数：", count)
 
# 将 ID 列表完整解码回原始文本
print("解码结果：", tokenizer.decode(token_ids))