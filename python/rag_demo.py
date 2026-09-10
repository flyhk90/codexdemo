# ------------------------------------------------------------
# 最小化 LangChain + Qwen2 RAG Demo
# 所有缓存（模型/向量库）统一放到 D:\hf_cache
# ------------------------------------------------------------

import os

# 1. 先设置 HuggingFace 缓存路径（在 import transformers 之前）
os.environ["HF_HOME"] = r"D:\hf_cache"
os.environ["HUGGINGFACE_HUB_CACHE"] = r"D:\hf_cache\hub"

from langchain_huggingface import HuggingFaceEmbeddings, HuggingFacePipeline
from langchain_community.vectorstores import FAISS
from langchain_text_splitters import RecursiveCharacterTextSplitter
from langchain_classic.chains import RetrievalQA

# ============================================================
# 2. 准备示例文档（模拟一个小型知识库）
# ============================================================
sample_text = """
LangChain 是一个用来把大模型包装成应用的开发框架。
它本身不训练模型，而是负责编排——把 LLM、外部数据、工具、记忆串成一条完整的工作流。

RAG（Retrieval-Augmented Generation，检索增强生成）是一种让大模型结合外部知识的技术。
它的核心思路是：先检索、再生成。
第一步，把文档切成小块并转换成向量，存入向量数据库。
第二步，收到用户问题时，先从向量库中检索相关的文档片段。
第三步，把检索到的文档片段和问题一起喂给大模型，让模型基于这些上下文生成答案。

FAISS 是 Meta 开源的向量搜索库，提供高性能的向量相似度检索。
它支持 CPU 和 GPU 两种运行模式，是最常用的向量数据库之一。

Qwen2 是阿里千问系列的开源大语言模型，支持中文和英文。
它在开源模型中表现优异，可通过 HuggingFace 库直接加载使用。

Embedding（嵌入）是将文本转换成固定维度向量的过程。
向量的每一维对应一个语义特征，语义相近的文本在向量空间中的距离更近。
"""

# ============================================================
# 3. 文档切分
# ============================================================
splitter = RecursiveCharacterTextSplitter(
    chunk_size=200,
    chunk_overlap=20,
)
chunks = splitter.create_documents([sample_text])
print(f"[切分] 生成 {len(chunks)} 个文档块")

# ============================================================
# 4. Embedding 模型（文本 → 向量）
# ============================================================
# 使用 bge-small-zh：轻量、中文友好、~100MB
# 下载缓存到 D:\hf_cache\hub
embeddings = HuggingFaceEmbeddings(
    model_name="BAAI/bge-small-zh-v1.5",
    cache_folder=r"D:\hf_cache\hub",
)

# ============================================================
# 5. 构建 FAISS 向量库
# ============================================================
faiss_index_path = r"D:\hf_cache\faiss_index"

# 如果已有索引则加载，否则创建新的
if os.path.exists(faiss_index_path):
    print("[FAISS] 加载已有索引...")
    vectorstore = FAISS.load_local(
        faiss_index_path, embeddings, allow_dangerous_deserialization=True
    )
else:
    print("[FAISS] 构建新索引...")
    vectorstore = FAISS.from_documents(chunks, embeddings)
    vectorstore.save_local(faiss_index_path)
    print(f"[FAISS] 索引已保存到 {faiss_index_path}")

# ============================================================
# 6. 加载 LLM（Qwen2-7B）
# ============================================================
print("[LLM] 加载 Qwen2-7B 模型（首次运行会下载，约 14GB）...")
llm = HuggingFacePipeline.from_model_id(
    model_id="Qwen/Qwen2-7B",
    task="text-generation",
    model_kwargs={
        "cache_dir": r"D:\hf_cache\hub",
        "device_map": "cpu",        # 无 GPU 时用 CPU
    },
    pipeline_kwargs={
        "max_new_tokens": 256,      # 生成的最大新 token 数
    },
)

# ============================================================
# 7. 构建 RAG 链
# ============================================================
qa_chain = RetrievalQA.from_chain_type(
    llm=llm,
    chain_type="stuff",              # 把所有检索结果塞进 prompt
    retriever=vectorstore.as_retriever(search_kwargs={"k": 3}),
    return_source_documents=True,
)

# ============================================================
# 8. 运行问答
# ============================================================
query = "什么是 RAG？它的工作流程是怎样的？"
print(f"\n[问题] {query}")
print("-" * 50)

result = qa_chain.invoke({"query": query})

print(f"[回答] {result['result']}")
print(f"\n[来源文档]")
for i, doc in enumerate(result["source_documents"], 1):
    print(f"  {i}. {doc.page_content[:80]}...")
d'f
print("\nDone.")
