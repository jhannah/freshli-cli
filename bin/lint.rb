#!/usr/bin/env ruby
# frozen_string_literal: true

require 'English'
require 'optparse'

require_relative './support/execute'

enable_dotnet_command_colors

perform_eclint = true
perform_rubocop = true
perform_dotnet_format = true
perform_codeclimate = true

parser = OptionParser.new do |options|
  options.banner = <<~BANNER
    Description:
        Linter Runner

    Usage:
        lint.rb [options]

    Options:
  BANNER

  options.on('--skip-eclint', 'Does not run the eclint linter') do
    perform_eclint = false
  end

  options.on('--skip-rubocop', 'Does not run the Rubocop linter') do
    perform_rubocop = false
  end

  options.on('--skip-dotnet-format', 'Does not run the dotnet format linter') do
    perform_dotnet_format = false
  end

  options.on('--skip-codeclimate', 'Does not run the codeclimate linter') do
    perform_codeclimate = false
  end

  options.on('-h', '--help', 'Show help and usage information') do
    puts options
    exit
  end
end

begin
  parser.parse!
rescue OptionParser::InvalidOption => e
  puts e
  puts parser
  exit(-1)
end

linter_failed = false

if perform_eclint
  status = execute('eclint')

  linter_failed = !status.success?
end

if perform_rubocop
  status = execute('bundle check > /dev/null')
  status = execute('bundle install') unless status.success?

  status = execute('bundle exec rubocop --color') if status.success?

  linter_failed = !status.success?
end

if perform_dotnet_format
  status = execute('dotnet format --verify-no-changes --severity info')

  linter_failed = !status.success?
end

if perform_codeclimate
  if ENV['DEVCONTAINER'] == 'true'
    status = execute(<<~COMMAND)
      sudo docker run \
        --rm \
        --env CODECLIMATE_CODE="#{ENV.fetch('CODE_FOLDER')}" \
        --volume "#{ENV.fetch('CODE_FOLDER')}":/code \
        --volume /var/run/docker.sock:/var/run/docker.sock \
        --volume /tmp/cc:/tmp/cc \
        codeclimate/codeclimate analyze
    COMMAND
  else
    status = execute('codeclimate analyze')
  end
  linter_failed = !status.success?
end

if linter_failed
  puts 'At least one linter encountered errors'
  exit(-1)
end
exit(0)