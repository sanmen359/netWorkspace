<template>
    <div :class="[
    type === 'textarea' ? 'el-textarea' : 'el-input',
    size ? 'el-input--' + size : '',
    {
      'is-disabled': disabled,
      'el-input-group': $slots.prepend || $slots.append,
      'el-input-group--append': $slots.append,
      'el-input-group--prepend': $slots.prepend
    }
  ]">
        <template v-if="type !== 'textarea'">
            <!-- 前置元素 -->
            <div class="el-input-group__prepend" v-if="$slots.prepend">
                <slot name="prepend"></slot>
            </div>
            <!-- input 图标 -->
            <slot name="icon">
                <i class="el-input__icon"
                   :class="[
            'el-icon-' + icon,
            onIconClick ? 'is-clickable' : ''
          ]"
                   v-if="icon"
                   @click="handleIconClick">
                </i>
            </slot>
            <input v-if="type !== 'textarea'"
                   class="el-input__inner"
                   v-bind="$props"
                   :autocomplete="autoComplete"
                   :value="currentValue"
                   ref="input"
                   @input="handleInput"
                   @focus="handleFocus"
                   @blur="handleBlur"
                   @keyup.enter="handleSearch"
                   >
            <i class="el-input__icon el-icon-loading" v-if="validating"></i>
            <!-- 后置元素 -->
            <div class="el-input-group__append" >
                <el-button icon="search" @click="handleSearch"></el-button>
            </div>
        </template>
        
    </div>
</template>
<script> 
  export default {
    name: 'KYSearch',

    componentName: 'KYSearch', 

    data() {
      return {
        currentValue: this.value,
        textareaCalcStyle: {}
      };
    },

    props: {
      value: [String, Number],
      placeholder: String,
      size: String,
      resize: String,
      readonly: Boolean,
      autofocus: Boolean,
      icon: String,
      disabled: Boolean,
      type: {
        type: String,
        default: 'text'
      },
      name: String,
      autosize: {
        type: [Boolean, Object],
        default: false
      },
      rows: {
        type: Number,
        default: 2
      },
      autoComplete: {
        type: String,
        default: 'off'
      },
      form: String,
      maxlength: Number,
      minlength: Number,
      max: {},
      min: {},
      step: {},
      validateEvent: {
        type: Boolean,
        default: true
      },
      onIconClick: Function,
      onSearch: Function
    },

    computed: {
      validating() {
        return this.$parent.validateState === 'validating';
      }
    },

    watch: {
      'value'(val, oldValue) {
        this.setCurrentValue(val);
      }
    },

    methods: { 
      handleBlur(event) {
        this.$emit('blur', event);
        if (this.validateEvent) {
          this.dispatch('ElFormItem', 'el.form.blur', [this.currentValue]);
        }
      },
      inputSelect() {
        this.$refs.input.select();
      },
      handleFocus(event) {
        this.$emit('focus', event);
      },
      handleInput(event) {
        const value = event.target.value;
        this.$emit('input', value);
        this.setCurrentValue(value);
        this.$emit('change', value);
      },
      handleIconClick(event) {
        if (this.onIconClick) {
          this.onIconClick(event);
        }
        this.$emit('click', event);
      },
      handleSearch(event) {
          if (this.onSearch) {
              this.onSearch(event);
          }
          this.$emit('search', event);
      },
      setCurrentValue(value) {
        if (value === this.currentValue) return;
        this.$nextTick(_ => {
          this.resizeTextarea();
        });
        this.currentValue = value;
        if (this.validateEvent) {
          this.dispatch('ElFormItem', 'el.form.change', [value]);
        }
      }
    },

    created() {
      this.$on('inputSelect', this.inputSelect);
    },
     
  };
</script>
