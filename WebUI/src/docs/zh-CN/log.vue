<style>
  .el-table .info-row {
    background: #c9e5f5;
  }

  .el-table .positive-row {
    background: #e2f0e4;
  }

  .demo-table .name-wrapper {
    display: inline-block;
  } 
 
</style>
<template>
<div> 

<el-row> 
    <div style="margin-top: 15px;margin-bottom:15px"> 
        <kw-search placeholder="请输入关键字" v-model="filter.keyword" @search="fetchData()">
            <template slot="prepend">
                请输入关键字
            </template>
        </kw-search>

    </div>
    <el-col :xs="24" :sm="24">
      <el-table
        :data="tableData"
        border
        style="width: 100%">
        <el-table-column 
          prop="time"
          label="时间"
          width="120">
        </el-table-column>
        <el-table-column
          prop="level"
          label="类型"
          width="150">
        </el-table-column>
        <el-table-column
          prop="logger"
          label="分类"
          width="120">
        </el-table-column> 
        <el-table-column
          prop="message"
          label="描述"
          width="200">
        </el-table-column>
        <el-table-column
          label="操作"
          width="140">
          <template scope="scope"> 
            <el-button
              size="small"
              @click="openDetail(scope.$index, scope.row)">查看详细信息</el-button>
         
          </template> 
        </el-table-column>
      </el-table>
        <el-pagination  
                       layout="prev, pager, next"
                       :page-size="filter.pageSize"
                       :current-page="filter.pageIndex"
                       @current-change="pageChange"
                       :total="pager.totalCount">
        </el-pagination>
    </el-col>
</el-row> 

 </div>
</template>

<script>
    export default {
        methods: {
            openDetail(index, data) {
                this.$alert(data.exception, "明细")
            },

            repError(r) {
                this.$alert(r.body, "Error", { type: "error" })
            },

            fetchData: function () {
                this.$http.post("admin/log/QueryCount", this.filter)
                    .then(response => {
                        this.pager.totalCount = response.body;
                    }, this.repError);
                this.pageChange(1)

            },
            pageChange(currentPage) {

                this.filter.pageIndex = currentPage;
                this.$http.post("admin/log/query", this.filter)
                    .then(response => {
                        this.tableData = response.body;
                    }, this.repError)
            }
        },
        mounted() {
            this.fetchData();
        },

        data() {
            return {
                tableData: [],
                filter: {
                    startTime: null,
                    endTime: null,
                    keyword: '',
                    pageIndex: 1,
                    pageSize: 10,
                },
                pager: {
                    totalCount: 0
                }
            }
        }
    }
</script>