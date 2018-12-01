module.exports = {
  root: true,
  env: {
    node: true,
    browser: true,
  },
  parser: 'babel-eslint',
  parserOptions: {
    sourceType: 'module',
    allowImportExportEverywhere: false,
    codeFrame: false,
  },
  extends: [
    'standard',
  ],
  // add your custom rules here
  rules: {
    // allow async-await
    'generator-star-spacing': 0,

    'brace-style': ['warn', '1tbs'],

    // allow unused variables in production
    'no-unused-vars': process.env.NODE_ENV === 'production' ? 0 : 1,
    // allow debugger during development
    'no-debugger': process.env.NODE_ENV === 'production' ? 2 : 0,
    'no-return-await': 'error',

    'object-curly-newline': ['warn', {
      ObjectExpression: { multiline: true, consistent: true },
      ObjectPattern: { multiline: true, consistent: true },
      ImportDeclaration: 'never',
      ExportDeclaration: { multiline: true, consistent: true, minProperties: 3 },
    }],
    'object-curly-spacing': ['warn', 'always'],

    'spaced-comment': ['error', 'always', {
      markers: ['#region', '#endregion'], // some IDE's will use //#region and //#endregion as foldable parts
    }],
    'space-infix-ops': 'warn',

    // allow trailing commas
    'comma-dangle': [process.env.NODE_ENV === 'production' ? 0 : 1, 'always-multiline'],
    'quote-props': [2, 'as-needed'],
    'no-var': 'warn',
    'object-shorthand': ['warn', 'always'],
    'prefer-promise-reject-errors': 'off',
    'prefer-const': 'warn',
    'no-mixed-operators': 'off',
  },
}
