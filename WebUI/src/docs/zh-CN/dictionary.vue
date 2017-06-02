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

  .demo-table .demo-table-expand {
    label {
      width: 90px;
      color: #99a9bf;
    }
    .el-form-item {
      margin-right: 0;
      margin-bottom: 0;
      width: 50%;
    }
  }

  .demo-box.demo-dialog {
    .dialog-footer button:first-child {
      margin-right: 10px;
    }
    .full-image {
      width: 100%;
    }
    .el-dialog__wrapper {
      margin: 0;
    }
    .el-select {
      width: 300px;
    }
    .el-input {
      width: 300px;
    }
    .el-button--text {
      margin-right: 15px;
    }
  }
</style>
<template>
<div> 
<el-row>
    <el-col :xs="24" :sm="8">
<div class="buttons">
  <el-button @click="addNode">添加根目录</el-button>
  <el-button @click="addChildNode">添加所选子分类</el-button>
  <el-button @click="editNode">修改分类</el-button>
  <el-button @click="removeNode">删除分类</el-button>
  <el-button @click="createItem">添加分类数据</el-button>
</div>
    <el-tree
      :data="data2"
      :props="defaultProps"
      node-key="id"
      :accordion="true"
      :auto-expand-parent='true'
      :highlight-current='true'
      :check-strictly='true'
      default-expand-all
      @node-click="nodeclick"
      :expand-on-click-node="false" >
    </el-tree>

    </el-col>
    <el-col :xs="24" :sm="16">
      <el-table
        :data="tableData"
        border
        style="width: 100%">
        <el-table-column
          fixed
          prop="code"
          label="编号"
          width="120">
        </el-table-column>
        <el-table-column
          prop="value"
          label="值"
          width="150">
        </el-table-column>
        <el-table-column
          prop="name"
          label="名称"
          width="120">
        </el-table-column> 
        <el-table-column
          prop="remark"
          label="描述"
          width="200">
        </el-table-column>
        <el-table-column
          fixed="right"
          label="操作"
          width="100">
          <template scope="scope"> 
            <el-button
              size="small"
              @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
            <el-button
              size="small"
              type="danger"
              @click="handleDelete(scope.$index, scope.row)">删除</el-button>
          </template> 
        </el-table-column>
      </el-table>
    </el-col>
</el-row> 
<el-dialog title="数据字典" v-model="dialogFormVisible">
<el-form ref="form" :model="form" label-width="80px">
  <el-form-item label="编码">
      <el-input v-model="form.code"></el-input>
  </el-form-item>
   <el-form-item label="名称">
    <el-input v-model="form.name"></el-input>
  </el-form-item>

  <el-form-item label="值">
      <el-input v-model="form.value"></el-input>
  </el-form-item>
  <el-form-item label="描述">
      <el-input v-model="form.remark"></el-input>
  </el-form-item>
</el-form>
  <div slot="footer" class="dialog-footer">
    <el-button @click="dialogFormVisible = false">取 消</el-button>
    <el-button type="primary" @click="onSubmit">确 定</el-button>
  </div>
</el-dialog>
 </div>
</template>

<script>
  export default {
    methods: {
      addNode(){
        var input=prompt('请输入名称');
        if(input){
          let tree= {
              id:'',
              label: input,
              type:'category',
              children: []
          };
        
          var url="Dictionary/AddTree";
          this.$http.post(url,{label:tree.label})
          .then(function(r){
            var newid=r.body;
            tree.id=newid;
            this.data2.push(tree);
          },this.repError)
        }else{
         this.repError({body:"请选择要删除的目录"})
        }
        
      },
      addChildNode(){
        if(this.currentTree.id){
          if(this.currentTree.type!='category'){
            this.repError({body:"请选择要删除的目录"})
            return;
          }
          var input=prompt('请输入名称');
          if(!input){
            return;
          }
          let tree= {
              id:'',
              label: input,
              type:'type',
              children: []
          };
          var url="Dictionary/AddTree/"+this.currentTree.id;
          this.$http.post(url,{label:tree.label})
          .then(function(r){
            var newid=r.body;
            tree.id=newid;
            this.currentTree.children.push(tree);
          },this.repError)
        }else{
         this.repError({body:"请选择要删除的目录"})
        }
      },
      createItem(){
        if(this.currentTree.id){
            if(this.currentTree.type!='type'){
              this.repError({body:"请选择要添加数据的分类"})
            }else{
              this.editIndex=null;
              this.dialogFormVisible=true; 
              this.form=this.getDefaultForm(); 
            }
        }
      },
      editNode(){
        var input=prompt('请填写名称',this.currentTree.label);
        if(input){
            this.currentTree.label=input;
            this.$http.post("dictionary/UpdateTree/"+this.currentTree.id,{label:input})
            .then(function(r){
              console.log(r.body);
            },this.repError)
        }else{
         this.repError({body:"请选择要删除的目录"})
        }
      },
      removeNode(){
        if(this.currentTree.id){
          this.$http.delete("dictionary/deleteTree/"+this.currentTree.id)
          .then(function(r){
            if(this.parents && this.parents.children){
              let i= this.parents.children.indexOf(this.currentTree);
              this.parents.children.splice(i,1);
            }else{
              let i= this.data2.indexOf(this.currentTree);
              this.data2.splice(i,1);
            }
          },this.repError)
         
        }else
        {
          this.repError({body:"请选择要删除的目录"})
        }
      },
      nodeclick(item,el,store){
          this.currentTree=item; 
          if(store.$parent.node){
            this.parents= store.$parent.node.data;
            this.getCurrentItems();
          }else{
            this.tableData=[];
          }
      },
      handleEdit(index, row) {  
        this.editIndex=index;
        this.dialogFormVisible=true;
        for(var r in row){
          this.form[r]=row[r];
        }
      },
      handleDelete(index, row) {
        console.log(index, row);
      },
      getCurrentItems(){
            this.$http.post("dictionary/GetItemByParentId",{pageIndex:0,pageSize:20,keyword:this.currentTree.id}).then(function(r){
              this.tableData=r.body;//.items;
            },this.repError)
      },
      onSubmit() {
        if(this.editIndex===null){
          this.form.parentId=this.currentTree.id;
          var data={
            parentId:this.currentTree.id,
            name:this.form.name,
            code: this.form.code,
            value: this.form.value,
            remark:this.form.remark
          }
          this.$http.post('dictionary/Post',data).then(function(r){
            this.getCurrentItems();

          },this.repError);
        }else{
          var currentModel=this.tableData[this.editIndex];
          for(var r in this.form){
            currentModel[r]=this.form[r];
          }
          this.$http.post('dictionary/PUT/'+this.form.id,this.form).then(function(r){
            this.getCurrentItems();
          },this.repError);
        }
        
        this.dialogFormVisible=false;
      },
      repError(r){
        this.$alert(r.body, "Error", { type: "error" })
      },
      getDefaultForm(){
        return  {
          id:'',
          parentId: '',
          name: '',
          code: '',
          value: '',
          type: 'Item',
          order: 0,
          remark:''
        };
      },
      fetchData: function () {
        this.$http.get("Dictionary/getTree")
                      .then(response => {
                          this.data2 = response.body;
                      },this.repError)
        }
    },
    mounted(){
      this.fetchData();
    },

    data() {
      return {
        dialogFormVisible:false,
        editIndex:0,
        currentTree:{},
        parents:[],
        form: {
          id:'',
          parentId: '',
          name: '',
          code: '',
          value: '',
          type: 'Item',
          order: 0,
          remark:''
        },
        tableData: [],
        data2: [
        ],
        defaultProps: {
          children: 'children',
          label: 'label'
        }
      }
    }
  }
</script>