<template>
    <div>
        <el-menu theme="dark" :default-active="'2-1'" class="el-menu-demo" mode="horizontal" >
            <el-menu-item index="1">处理中心</el-menu-item>
            <el-submenu index="2">
                <template slot="title">
                    我的工作台
                </template>
                <el-menu-item index="2-1">选项1</el-menu-item>
                <el-menu-item index="2-2">选项2</el-menu-item>
                <el-menu-item index="2-3">选项3</el-menu-item>
            </el-submenu>
            <el-menu-item index="3"><a href="https://www.ele.me" target="_blank">订单管理</a></el-menu-item>
        </el-menu>
        <div class="line"></div>
        <el-row>
            <el-col :xs="24" :sm="6">
                <el-menu default-active="/zh-CN/home"  class="el-menu-vertical-demo" :router='useRouter'>
                <template v-for="nav in navsData">
                 <el-submenu  :index="'/'+lang+nav.path" >
                    <template slot="title">
                        {{nav.groupName}}
                    </template>
                    <template v-for="item in nav.list">
                        <el-menu-item :index="'/'+lang+item.path" >{{item.title}}
                        </el-menu-item>
                    </template>
                </el-submenu> 
                </template>
               
                </el-menu>  
            </el-col>
            <el-col :xs="24" :sm="18">
                <router-view></router-view>
            </el-col>
        </el-row>
    </div>
</template>
<script>
   import navsData from './nav.config.json';
   
   export default {
    data() {
      return {
        lang:"zh-CN",
        navsData:navsData["zh-CN"],
        useRouter:true
      };
    }
  };
</script>
<style>
    body {
        font-family: Helvetica, sans-serif;
    }
</style>
